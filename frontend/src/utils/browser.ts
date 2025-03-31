/**
 * 浏览器信息接口
 */
export interface BrowserInfo {
  browser: string
  browserVersion: string
  os: string
  osVersion: string
  deviceType: number
  deviceName: string
  screen: {
    width: number
    height: number
    colorDepth: number
    pixelRatio: number
    availWidth: number
    availHeight: number
  }
  hardware: {
    maxTouchPoints: number
    hardwareConcurrency: number
    deviceMemory: number
    platform: string
    vendor: string
  }
  locale: {
    language: string
    timezone: string
    languages: readonly string[]
  }
  userAgent: string
  isSupported: boolean
  unsupportedMessage: string
  author: {
    name: string
    email: string
    website: string
    copyright: string
  }
}

/**
 * 获取浏览器信息
 */
export function getBrowserInfo(unsupportedMessages: {
  browser: string
  windows: string
  macos: string
  linux: string
  android: string
  ios: string
  os: string
}): BrowserInfo {
  const ua = navigator.userAgent || ''

  // 检测浏览器类型和版本
  let browser = ''
  let browserVersion = ''
  let os = ''
  let osVersion = ''
  let deviceType = 0
  let deviceName = ''
  let isSupported = true
  let unsupportedMessage = ''

  // 主流浏览器检测
  if (ua.includes('Edg')) {
    browser = 'Edge'
    browserVersion = ua.match(/Edg\/([0-9.]+)/)?.[1] || ''
  }
  else if (ua.includes('Chrome')) {
    browser = 'Chrome'
    browserVersion = ua.match(/Chrome\/([0-9.]+)/)?.[1] || ''
  }
  else if (ua.includes('Firefox')) {
    browser = 'Firefox'
    browserVersion = ua.match(/Firefox\/([0-9.]+)/)?.[1] || ''
  }
  else if (ua.includes('Safari') && !ua.includes('Chrome')) {
    browser = 'Safari'
    browserVersion = ua.match(/Version\/([0-9.]+)/)?.[1] || ''
  }
  else if (ua.includes('Opera')) {
    browser = 'Opera'
    browserVersion = ua.match(/Opera\/([0-9.]+)/)?.[1] || ''
  }
  else if (ua.includes('Brave')) {
    browser = 'Brave'
    browserVersion = ua.match(/Brave\/([0-9.]+)/)?.[1] || ''
  }
  else if (ua.includes('Vivaldi')) {
    browser = 'Vivaldi'
    browserVersion = ua.match(/Vivaldi\/([0-9.]+)/)?.[1] || ''
  }
  else {
    isSupported = false
    browser = 'OtherBrowser'
    browserVersion = '0.0'
    unsupportedMessage = unsupportedMessages.browser
  }

  // 主流操作系统检测
  if (ua.includes('Windows')) {
    os = 'Windows'
    if (ua.includes('Windows NT 11.0')) {
      osVersion = '11'
    }
    else if (ua.includes('Windows NT 10.0')) {
      osVersion = '10'
    }
    else if (ua.includes('Windows NT 6.3')) {
      osVersion = '8.1'
    }
    else if (ua.includes('Windows NT 6.2')) {
      osVersion = '8'
    }
    else if (ua.includes('Windows NT 6.1')) {
      osVersion = '7'
    }
    else {
      isSupported = false
      osVersion = '0'
      unsupportedMessage = unsupportedMessages.windows
    }
  }
  else if (ua.includes('Mac')) {
    os = 'macOS'
    if (ua.includes('Mac OS X 14')) {
      osVersion = 'Sonoma'
    }
    else if (ua.includes('Mac OS X 13')) {
      osVersion = 'Ventura'
    }
    else if (ua.includes('Mac OS X 12')) {
      osVersion = 'Monterey'
    }
    else if (ua.includes('Mac OS X 11')) {
      osVersion = 'Big Sur'
    }
    else {
      isSupported = false
      osVersion = '0'
      unsupportedMessage = unsupportedMessages.macos
    }
  }
  else if (ua.includes('Linux')) {
    os = 'Linux'
    if (ua.includes('Ubuntu')) {
      osVersion = 'Ubuntu'
    }
    else if (ua.includes('Debian')) {
      osVersion = 'Debian'
    }
    else if (ua.includes('Fedora')) {
      osVersion = 'Fedora'
    }
    else if (ua.includes('CentOS')) {
      osVersion = 'CentOS'
    }
    else if (ua.includes('Arch')) {
      osVersion = 'Arch'
    }
    else {
      isSupported = false
      osVersion = 'OtherLinux'
      unsupportedMessage = unsupportedMessages.linux
    }
  }
  else if (ua.includes('Android')) {
    os = 'Android'
    const version = ua.match(/Android ([0-9.]+)/)?.[1] || '0'
    osVersion = version
    if (parseInt(version) < 11) {
      isSupported = false
      unsupportedMessage = unsupportedMessages.android
    }
  }
  else if (ua.includes('iPhone') || ua.includes('iPad')) {
    os = 'iOS'
    const version = ua.match(/OS ([0-9_]+)/)?.[1]?.replace(/_/g, '.') || '0'
    osVersion = version
    if (parseInt(version) < 14) {
      isSupported = false
      unsupportedMessage = unsupportedMessages.ios
    }
  }
  else {
    isSupported = false
    os = 'OtherOS'
    osVersion = '0'
    unsupportedMessage = unsupportedMessages.os
  }

  // 检测设备类型
  const isMobile = /Mobile|Android|iPhone/i.test(ua)
  deviceType = isMobile ? 1 : 0

  // 获取设备名称
  if (isMobile) {
    if (ua.includes('iPhone')) {
      deviceName = 'iPhone'
    }
    else if (ua.includes('iPad')) {
      deviceName = 'iPad'
    }
    else if (ua.includes('Android')) {
      deviceName = 'Android Device'
    }
    else {
      deviceName = 'Mobile Device'
    }
  }
  else {
    deviceName = `${os} Desktop`
  }

  // 获取屏幕信息
  let screenWidth = 0
  let screenHeight = 0
  let screenColorDepth = 24
  let screenPixelRatio = 1
  let screenAvailWidth = 0
  let screenAvailHeight = 0

  try {
    // 优先使用 window.screen 的信息
    screenWidth = window.screen?.width || window.innerWidth || 0
    screenHeight = window.screen?.height || window.innerHeight || 0
    screenColorDepth = window.screen?.colorDepth || 24
    screenPixelRatio = window.devicePixelRatio || 1
    screenAvailWidth = window.screen?.availWidth || window.innerWidth || 0
    screenAvailHeight = window.screen?.availHeight || window.innerHeight || 0

    // 确保分辨率是有效的
    if (screenWidth <= 0 || screenHeight <= 0) {
      screenWidth = window.screen?.width || 1920
      screenHeight = window.screen?.height || 1080
    }
  }
  catch (error) {
    console.warn('获取屏幕信息失败:', error)
    // 设置默认值
    screenWidth = 1920
    screenHeight = 1080
    screenColorDepth = 24
    screenPixelRatio = 1
    screenAvailWidth = 1920
    screenAvailHeight = 1080
  }

  const screen = {
    width: screenWidth,
    height: screenHeight,
    colorDepth: screenColorDepth,
    pixelRatio: screenPixelRatio,
    availWidth: screenAvailWidth,
    availHeight: screenAvailHeight
  }

  // 获取硬件信息
  const hardware = {
    maxTouchPoints: navigator.maxTouchPoints ?? 0,
    hardwareConcurrency: navigator.hardwareConcurrency ?? 0,
    deviceMemory: (navigator as any).deviceMemory ?? 0,
    platform: navigator.platform ?? os,
    vendor: navigator.vendor ?? browser
  }

  // 获取语言和时区信息
  const locale = {
    language: navigator.language ?? 'zh-CN',
    timezone: Intl.DateTimeFormat?.().resolvedOptions?.().timeZone ?? 'Asia/Shanghai',
    languages: navigator.languages ?? ['zh-CN']
  }

  // 获取作者信息
  const author = {
    name: 'HBT Code Generator Team',
    email: 'support@hbtcodegen.com',
    website: 'https://hbtcodegen.com',
    copyright: `© ${new Date().getFullYear()} HBT Code Generator. All rights reserved.`
  }

  const browserInfo = {
    browser,
    browserVersion,
    os,
    osVersion,
    deviceType,
    deviceName,
    screen,
    hardware,
    locale,
    userAgent: ua,
    isSupported,
    unsupportedMessage,
    author
  }

  // 输出详细的浏览器信息日志
  console.group('Browser Information')
  console.log('User Agent:', ua)
  console.log('Browser:', browser, browserVersion)
  console.log('OS:', os, osVersion)
  console.log('Device:', {
    type: deviceType,
    name: deviceName,
    isMobile: deviceType === 1
  })
  console.log('Screen:', {
    resolution: `${screen.width}x${screen.height}`,
    colorDepth: screen.colorDepth,
    pixelRatio: screen.pixelRatio,
    available: `${screen.availWidth}x${screen.availHeight}`
  })
  console.log('Hardware:', {
    touchPoints: hardware.maxTouchPoints,
    cores: hardware.hardwareConcurrency,
    memory: hardware.deviceMemory,
    platform: hardware.platform,
    vendor: hardware.vendor
  })
  console.log('Locale:', {
    language: locale.language,
    timezone: locale.timezone,
    languages: locale.languages
  })
  console.log('Compatibility:', {
    isSupported,
    message: unsupportedMessage
  })
  console.log('Author:', author)
  console.groupEnd()

  return browserInfo
} 