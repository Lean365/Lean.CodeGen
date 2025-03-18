export default {
  /** Common Actions */
  actions: {
    /** Basic Operations */
    create: 'Create',
    update: 'Update',
    delete: 'Delete',
    preview: 'Preview',
    detail: 'Detail',
    save: 'Save',
    submit: 'Submit',
    cancel: 'Cancel',
    confirm: 'Confirm',
    back: 'Back',
    
    /** Query Operations */
    search: 'Search',
    filter: 'Filter',
    reset: 'Reset',
    refresh: 'Refresh',
    
    /** Data Operations */
    import: 'Import',
    export: 'Export',
    download: 'Download',
    upload: 'Upload',
    copy: 'Copy',
    paste: 'Paste',
    print: 'Print',
    
    /** List Operations */
    more: 'More',
    expand: 'Expand',
    collapse: 'Collapse',
    next: 'Next',
    previous: 'Previous',
    
    /** Other Operations */
    enable: 'Enable',
    disable: 'Disable',
    config: 'Configure',
    settings: 'Settings',
    help: 'Help',
    about: 'About'
  },

  /** Common Status */
  status: {
    loading: 'Loading',
    success: 'Success',
    error: 'Error',
    warning: 'Warning',
    info: 'Info',
    
    /** Process Status */
    process: {
      pending: 'Pending',
      processing: 'Processing',
      completed: 'Completed',
      failed: 'Failed',
      canceled: 'Canceled'
    }
  },

  /** Form Related */
  form: {
    /** Required and Optional */
    required: 'Required',
    optional: 'Optional',
    
    /** Input Prompts */
    pleaseSelect: 'Please Select',
    pleaseInput: 'Please Input',
    pleaseConfirm: 'Please Confirm',
    pleaseUpload: 'Please Upload',
    
    /** Validation Tips */
    invalid: 'Invalid Input',
    formatError: 'Format Error',
    lengthError: 'Length Error',
    rangeError: 'Range Error',
    duplicated: 'Duplicated Data',
    
    /** Selection Types */
    single: 'Single Select',
    multiple: 'Multiple Select',
    all: 'Select All',
    none: 'Select None',
    
    /** File Related */
    file: {
      select: 'Select File',
      upload: 'Upload File',
      preview: 'Preview File',
      download: 'Download File',
      delete: 'Delete File',
      size: 'File Size',
      type: 'File Type',
      name: 'File Name'
    },
    
    /** Image Related */
    image: {
      select: 'Select Image',
      upload: 'Upload Image',
      preview: 'Preview Image',
      crop: 'Crop Image',
      rotate: 'Rotate Image',
      delete: 'Delete Image'
    }
  },

  /** Basic Options */
  options: {
    yes: 'Yes',
    no: 'No'
  },

  http: {
    error: {
      default: 'Request failed',
      network: 'Network error',
      timeout: 'Request timeout',
      config: 'Request configuration error',
      serverError: 'Server error',
      unknown: 'Unknown error',
      unauthorized: 'Unauthorized, please login again'
    },
    status: {
      400: 'Bad request',
      401: 'Unauthorized, please login again',
      402: 'Payment required',
      403: 'Access denied',
      404: 'Resource not found',
      405: 'Method not allowed',
      406: 'Not acceptable',
      408: 'Request timeout',
      413: 'Payload too large',
      415: 'Unsupported media type',
      422: 'Unprocessable entity',
      423: 'Resource locked',
      428: 'Precondition required',
      429: 'Too many requests',
      500: 'Internal server error',
      501: 'Not implemented',
      502: 'Bad gateway',
      503: 'Service unavailable',
      504: 'Gateway timeout',
      505: 'HTTP version not supported'
    },
    business: {
      success: 'Operation successful',
      error: 'Operation failed',
      networkError: 'Network connection error',
      serverError: 'Server response error',
      requestError: 'Request parameter error',
      optionError: 'Request option error',
      rateLimit: 'Request rate limit exceeded, remaining requests: {remaining}'
    }
  },

  /** Page Header */
  header: {
    language: 'Switch Language',
    theme: 'Switch Theme',
    profile: 'Profile',
    settings: 'Settings',
    logout: 'Logout'
  },

  /** Page Footer */
  footer: {
    copyright: '© {year} HBT Code Generator. All rights reserved.',
    about: 'About Us',
    privacy: 'Privacy Policy',
    terms: 'Terms of Service'
  }
} 