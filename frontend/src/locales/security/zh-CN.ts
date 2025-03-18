export default {
  security: {
    sql: {
      riskDetected: '检测到不安全的输入',
      riskCharsDetected: '检测到高风险字符',
      possibleRisk: '输入可能存在风险',
      cleaningFailed: '输入清理失败',
      validation: {
        failed: '输入验证失败',
        success: '输入验证通过'
      }
    },
    xsrf: {
      tokenMissing: 'XSRF 令牌缺失',
      tokenInvalid: 'XSRF 令牌无效',
      tokenRefreshed: 'XSRF 令牌已刷新',
      validation: {
        failed: '令牌验证失败',
        success: '令牌验证通过'
      }
    },
    rateLimit: {
      exceeded: '请求频率超出限制',
      remaining: '剩余可用请求次数: {count}',
      nextAvailable: '下次可用时间: {time}',
      reset: '限制已重置'
    }
  }
} 