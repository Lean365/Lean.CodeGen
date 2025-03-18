export default {
  login: {
    title: {
      text: 'User Login',
      description: 'Welcome to Code Generator'
    },
    form: {
      username: {
        label: 'Username',
        placeholder: 'Please enter username',
        required: 'Please enter username'
      },
      password: {
        label: 'Password',
        placeholder: 'Please enter password',
        required: 'Please enter password'
      },
      remember: {
        text: 'Remember me',
        tip: 'Auto login within 7 days'
      },
      submit: {
        text: 'Login',
        loading: 'Logging in...'
      }
    },
    links: {
      forgot: {
        text: 'Forgot password?',
        tip: 'Please contact administrator to reset password'
      },
      terms: {
        text: 'Terms of Service',
        tip: 'View Terms of Service'
      },
      privacy: {
        text: 'Privacy Policy',
        tip: 'View Privacy Policy'
      }
    },
    messages: {
      success: 'Login successful',
      failed: 'Login failed',
      validateFailed: 'Please check your input',
      accountLocked: 'Account is locked',
      accountDisabled: 'Account is disabled',
      accountExpired: 'Account has expired',
      passwordExpired: 'Password has expired'
    }
  }
} 