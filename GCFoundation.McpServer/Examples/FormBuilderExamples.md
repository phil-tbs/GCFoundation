# GCFoundation Form Builder Examples

## 1. Simple Contact Form

Use the `GenerateCompleteFormSolution` MCP function with these parameters:

**Form Name:** `ContactForm`
**Fields JSON:**
```json
[
  {
    "name": "firstName",
    "type": "text",
    "label": "First Name",
    "required": true,
    "maxLength": 50
  },
  {
    "name": "lastName",
    "type": "text",
    "label": "Last Name",
    "required": true,
    "maxLength": 50
  },
  {
    "name": "email",
    "type": "email",
    "label": "Email Address",
    "required": true,
    "validation": "email"
  },
  {
    "name": "message",
    "type": "textarea",
    "label": "Message",
    "required": true,
    "rows": 5,
    "maxLength": 1000,
    "hint": "Please describe your inquiry"
  }
]
```

**Result:** Complete contact form with model, controller, and view files.

## 2. Advanced Multi-Step Job Application

Use the `GenerateAdvancedFormSolution` MCP function with these parameters:

**Form Name:** `JobApplication`
**Steps JSON:**
```json
[
  {
    "stepName": "Personal",
    "title": "Personal Information",
    "fields": [
      {
        "name": "firstName",
        "type": "text",
        "label": "First Name",
        "required": true,
        "maxLength": 50
      },
      {
        "name": "lastName",
        "type": "text",
        "label": "Last Name",
        "required": true,
        "maxLength": 50
      },
      {
        "name": "email",
        "type": "email",
        "label": "Email Address",
        "required": true,
        "validation": "email"
      },
      {
        "name": "phone",
        "type": "tel",
        "label": "Phone Number",
        "required": true,
        "validation": "phone"
      }
    ]
  },
  {
    "stepName": "Professional",
    "title": "Professional Experience",
    "fields": [
      {
        "name": "currentPosition",
        "type": "text",
        "label": "Current Position",
        "required": true
      },
      {
        "name": "yearsExperience",
        "type": "number",
        "label": "Years of Experience",
        "required": true
      },
      {
        "name": "hasSecurityClearance",
        "type": "checkbox",
        "label": "Do you have security clearance?",
        "required": false
      },
      {
        "name": "clearanceLevel",
        "type": "select",
        "label": "Security Clearance Level",
        "required": false,
        "conditionalOn": "hasSecurityClearance",
        "conditionalValue": "true",
        "hint": "Select your highest clearance level"
      }
    ]
  },
  {
    "stepName": "Documents",
    "title": "Supporting Documents",
    "fields": [
      {
        "name": "resume",
        "type": "file",
        "label": "Resume/CV",
        "required": true,
        "accept": ".pdf,.doc,.docx",
        "hint": "Upload your resume in PDF or Word format"
      },
      {
        "name": "coverLetter",
        "type": "file",
        "label": "Cover Letter",
        "required": false,
        "accept": ".pdf,.doc,.docx"
      }
    ]
  }
]
```

**Features:**
- Multi-step navigation with progress tracking
- Session storage for data persistence
- Conditional fields (clearance level only shows if user has clearance)
- File upload capabilities
- Step-by-step validation
- Review page before submission

## 3. Government Service Request Form

Use the `GenerateCompleteFormSolution` MCP function with file upload:

**Form Name:** `ServiceRequest`
**Include File Upload:** `true`
**Fields JSON:**
```json
[
  {
    "name": "serviceType",
    "type": "select",
    "label": "Type of Service",
    "required": true,
    "hint": "Select the type of service you are requesting"
  },
  {
    "name": "requestDescription",
    "type": "textarea",
    "label": "Request Description",
    "required": true,
    "rows": 6,
    "maxLength": 2000,
    "hint": "Please provide detailed information about your request"
  },
  {
    "name": "urgencyLevel",
    "type": "select",
    "label": "Urgency Level",
    "required": true
  },
  {
    "name": "clientId",
    "type": "text",
    "label": "Client ID",
    "required": true,
    "maxLength": 20,
    "placeholder": "Enter your client identification number"
  },
  {
    "name": "preferredContact",
    "type": "select",
    "label": "Preferred Contact Method",
    "required": true
  }
]
```

## 4. Using the MCP Functions

### Via AI Assistant (Claude, ChatGPT, etc.)

Simply provide the function name and parameters to your AI assistant that has access to the GCFoundation MCP server:

```
Please use the GenerateCompleteFormSolution function to create a contact form with firstName, lastName, email, and message fields.
```

### Direct Usage

If calling the MCP server directly, use the tool name and provide the JSON parameters as specified in the examples above.

## 5. Integration with GCFoundation

The generated forms will automatically include:

- **GCDS Components:** All form elements use proper Government of Canada Design System components
- **Accessibility:** Built-in WCAG compliance and screen reader support
- **Validation:** Server-side and client-side validation with proper error handling
- **Localization Ready:** Structure supports English/French bilingual applications
- **Security:** Anti-forgery tokens and proper validation
- **Error Handling:** Comprehensive error summaries and field-level validation

## 6. Customization Options

After generation, you can customize:

- **Styling:** Add custom CSS classes while maintaining GCDS compliance
- **Business Logic:** Add your specific processing logic in the controller actions
- **Validation:** Extend validation rules in the model or controller
- **Workflow:** Modify the form flow to match your specific requirements
- **Integration:** Connect to your existing services and databases

## 7. Best Practices

- **Always use the generated base structure** and extend rather than rewrite
- **Test accessibility** with screen readers after customization
- **Follow GC Design System guidelines** for any custom styling
- **Implement proper error logging** in the controller actions
- **Add unit tests** for your model validation and controller logic
- **Consider localization** from the beginning if building bilingual applications
