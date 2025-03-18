export default {
  security: {
    sql: {
      riskDetected: 'Unsafe input detected',
      riskCharsDetected: 'High-risk characters detected',
      possibleRisk: 'Input may contain risks',
      cleaningFailed: 'Input cleaning failed',
      validation: {
        failed: 'Input validation failed',
        success: 'Input validation passed'
      }
    },
    xsrf: {
      tokenMissing: 'XSRF token missing',
      tokenInvalid: 'XSRF token invalid',
      tokenRefreshed: 'XSRF token refreshed',
      validation: {
        failed: 'Token validation failed',
        success: 'Token validation passed'
      }
    },
    rateLimit: {
      exceeded: 'Request rate limit exceeded',
      remaining: 'Remaining requests available: {count}',
      nextAvailable: 'Next available time: {time}',
      reset: 'Rate limit reset'
    }
  }
} 