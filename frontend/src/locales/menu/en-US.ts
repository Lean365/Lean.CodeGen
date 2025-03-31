export default {
  menu: {
    dashboard: {
      _self: 'Dashboard',
      index: 'Home',
      analysis: 'Analysis',
      monitor: 'Monitor'
    },
    about: {
      _self: 'About',
      index: 'About',
      terms: 'Terms of Service',
      privacy: 'Privacy Policy'
    },

    // ================================
    // Identity Module
    // ================================
    identity: {
      _self: 'Identity',                    // Module name
      user: 'User Management',          // User management page
      role: 'Role Management',          // Role management page
      menu: 'Menu Management',          // Menu management page
      dept: 'Department Management',    // Department management page
      post: 'Post Management'           // Position management page
    },

    // ================================
    // Administration Module
    // ================================
    admin: {
      _self: 'System Management',              // Module name
      dicttype: 'Dictionary Type',      // Dictionary type page
      dictdata: 'Dictionary Data',      // Dictionary data page
      config: 'Parameter Configuration',          // Configuration page
      language: 'Language Management',             // Language management page
      translation: 'Translation Management',       // Translation management page
      localization: 'Localization'      // Localization page
    },

    // ================================
    // Code Generator Module
    // ================================
    generator: {
      _self: 'Code Generation',              // Module name
      gentask: 'Generation Task',       // Generation task page
      gentemplate: 'Generation Template', // Generation template page
      genconfig: 'Generation Config',   // Generation config page
      genhistory: 'Generation History', // Generation history page
      datasource: 'Data Source',        // Data source page
      dbtable: 'Database Table',        // Database table page
      tableconfig: 'Table Configuration'       // Table config page
    },

    // ================================
    // Workflow Module
    // ================================
    workflow: {
      _self: 'Workflow',                           // Module name
      workflowdefinition: 'Process Definition',        // Process definition page
      workflowinstance: 'Process Instance',            // Process instance page
      workflowtask: 'Process Task',                    // Process task page
      workflowform: 'Process Form',                    // Process form page
      workflowvariable: 'Process Variable',            // Process variable page
      workflowvariabledata: 'Variable Data',          // Variable data page
      workflowactivitytype: 'Activity Type',          // Activity type page
      workflowactivityproperty: 'Activity Property',  // Activity property page
      workflowactivityinstance: 'Activity Instance',  // Activity instance page
      workflowoutput: 'Process Output',               // Process output page
      workflowoutcome: 'Process Outcome',             // Process outcome page
      workflowhistory: 'Process History',             // Process history page
      workflowcorrelation: 'Process Correlation'       // Process correlation page
    },

    // ================================
    // Real-time Communication Module
    // ================================
    signalr: {
      _self: 'Real-time Communication',                  // Module name
      onlineuser: 'Online Users',      // Online users page
      onlinemessage: 'Online Messages' // Online messages page
    },

    // ================================
    // Audit Module
    // ================================
    audit: {
      _self: 'Audit Log',                      // Module name
      auditlog: 'Audit Log',           // Audit log page
      operationlog: 'Operation Log',    // Operation log page
      loginlog: 'Login Log',           // Login log page
      exceptionlog: 'Exception Log',    // Exception log page
      sqldifflog: 'SQL Diff Log'       // SQL diff log page
    }
  },

  // ================================
  // Buttons
  // ================================
  button: {
    query: 'Query',        // Query button
    create: 'Create',      // Create button
    update: 'Update',      // Update button
    delete: 'Delete',      // Delete button
    clear: 'Clear',        // Clear button
    template: 'Template',  // Template button
    import: 'Import',      // Import button
    export: 'Export',      // Export button
    preview: 'Preview',    // Preview button
    print: 'Print',        // Print button
    audit: 'Audit',        // Audit button
    revoke: 'Revoke',      // Revoke button
    translate: 'Translate', // Translate button
    icon: 'Icon'           // Icon button
  }
} 