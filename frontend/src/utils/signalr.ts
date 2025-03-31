import { HubConnectionBuilder, HubConnection, LogLevel, HttpTransportType } from '@microsoft/signalr'
import { useOnlineUserStore } from '@/stores/signalr/onlineUser'
import { useOnlineMessageStore } from '@/stores/signalr/onlineMessage'
import type { LeanOnlineMessage } from '@/types/signalr/onlineMessage'
import type { LeanOnlineUser } from '@/types/signalr/onlineUser'
import type { LeanApiResult } from '@/types/common/api'
import { LeanErrorCode } from '@/types/common/errorCode'
import { LeanBusinessType } from '@/types/common/businessType'

class SignalRService {
  private connection: HubConnection | null = null
  private onlineUserStore: ReturnType<typeof useOnlineUserStore> | null = null
  private onlineMessageStore: ReturnType<typeof useOnlineMessageStore> | null = null

  // 连接状态
  private isConnected = false
  // 重连次数
  private reconnectCount = 0
  // 最大重连次数
  private readonly maxReconnectCount = 5
  // 心跳定时器
  private heartbeatTimer: number | null = null
  // 心跳间隔（毫秒）
  private readonly heartbeatInterval = 30000

  constructor() {
    // 移除构造函数中的 store 初始化
  }

  // 初始化连接
  public async initConnection(): Promise<void> {
    if (this.connection) {
      return
    }

    try {
      // 初始化 store
      this.onlineUserStore = useOnlineUserStore()
      this.onlineMessageStore = useOnlineMessageStore()

      this.connection = new HubConnectionBuilder()
        .withUrl('http://localhost:5152/signalr/hubs')
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .configureLogging(LogLevel.Information)
        .build()

      // 注册事件处理
      this.registerEventHandlers()

      // 启动连接
      await this.connection.start()
      this.isConnected = true
      this.reconnectCount = 0

      // 启动心跳检测
      this.startHeartbeat()

      console.log('SignalR连接成功')
    } catch (error) {
      console.error('SignalR连接失败:', error)
      this.handleConnectionError()
    }
  }

  // 注册事件处理
  private registerEventHandlers(): void {
    if (!this.connection) return

    // 连接关闭事件
    this.connection.onclose(() => {
      this.isConnected = false
      this.stopHeartbeat()
      console.log('SignalR连接关闭')
    })

    // 重连事件
    this.connection.onreconnecting((error) => {
      this.reconnectCount++
      console.log(`SignalR重连中 (${this.reconnectCount}/${this.maxReconnectCount}):`, error)
    })

    // 重连成功事件
    this.connection.onreconnected((connectionId) => {
      this.isConnected = true
      this.reconnectCount = 0
      this.startHeartbeat()
      console.log('SignalR重连成功:', connectionId)
    })

    // 接收未读消息
    this.connection.on('ReceiveUnreadMessages', (messages: LeanOnlineMessage[]) => {
      this.onlineMessageStore?.setUnreadMessages(messages)
    })

    // 接收用户登录尝试通知
    this.connection.on('ReceiveUserLoginAttempt', (message: string, loginTime: string, loginIp: string, loginLocation: string) => {
      console.log('收到登录尝试通知:', { message, loginTime, loginIp, loginLocation })
    })

    // 接收通知
    this.connection.on('ReceiveNotification', (notification: any) => {
      console.log('收到通知:', notification)
    })
  }

  // 启动心跳检测
  private startHeartbeat(): void {
    if (this.heartbeatTimer) {
      clearInterval(this.heartbeatTimer)
    }

    this.heartbeatTimer = setInterval(async () => {
      if (this.isConnected && this.connection) {
        try {
          await this.connection.invoke('HeartbeatAsync')
        } catch (error) {
          console.error('心跳检测失败:', error)
          this.handleConnectionError()
        }
      }
    }, this.heartbeatInterval)
  }

  // 停止心跳检测
  private stopHeartbeat(): void {
    if (this.heartbeatTimer) {
      clearInterval(this.heartbeatTimer)
      this.heartbeatTimer = null
    }
  }

  // 处理连接错误
  private handleConnectionError(): void {
    this.isConnected = false
    this.stopHeartbeat()

    if (this.reconnectCount >= this.maxReconnectCount) {
      console.error('SignalR重连次数超过最大限制，停止重连')
      return
    }

    // 延迟重连
    setTimeout(() => {
      this.initConnection()
    }, 5000)
  }

  // 关闭连接
  public async closeConnection(): Promise<void> {
    this.stopHeartbeat()
    if (this.connection) {
      try {
        await this.connection.stop()
        this.connection = null
        this.isConnected = false
        console.log('SignalR连接已关闭')
      } catch (error) {
        console.error('关闭SignalR连接失败:', error)
      }
    }
  }

  // 调用 SignalR 方法
  public async invoke<T>(method: string, ...args: any[]): Promise<LeanApiResult<T>> {
    if (!this.connection) {
      return {
        success: false,
        code: LeanErrorCode.SystemError,
        message: 'SignalR未连接',
        businessType: LeanBusinessType.Other,
        timestamp: Date.now(),
        data: null
      }
    }

    try {
      const result = await this.connection.invoke(method, ...args)
      return {
        success: true,
        code: LeanErrorCode.Status200OK,
        message: null,
        businessType: LeanBusinessType.Other,
        timestamp: Date.now(),
        data: result
      }
    } catch (error) {
      return {
        success: false,
        code: LeanErrorCode.SystemError,
        message: 'SignalR调用失败',
        businessType: LeanBusinessType.Other,
        timestamp: Date.now(),
        data: null
      }
    }
  }
}

export const signalRService = new SignalRService()