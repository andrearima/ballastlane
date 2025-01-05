# User Story: Manage Contacts for Authenticated Users

### **As a**  
Authenticated user of the system.

### **I want to**  
Perform CRUD (Create, Read, Update, Delete) operations on my contacts.

### **So that I can**  
Manage my personal contact list efficiently.

---

## Acceptance Criteria

### **Scenario 1: Retrieve a Contact**  

**Given**  
I am an authenticated user with a valid `UserId`.

**When**  
I send a `GET` request to `/contact/{contactId}` with a valid `contactId`.

**Then**  
- I should receive a `200 OK` response with the contact's details.
- If the `contactId` is not found, I should receive a `404 Not Found` response.

---

### **Scenario 2: Create a Contact**

**Given**  
I am an authenticated user with a valid `UserId`.

**When**  
I send a `POST` request to `/contact` with valid contact details in the request body.

**Then**  
- I should receive a `200 OK` response with the created contact's details.
- If the request body is invalid or missing, I should receive a `400 Bad Request` response.

---

### **Scenario 3: Update a Contact**

**Given**  
I am an authenticated user with a valid `UserId` and a valid `contactId`.

**When**  
I send a `PUT` request to `/contact/{contactId}` with valid updated contact details in the request body.

**Then**  
- I should receive a `200 OK` response with the updated contact's details.
- If the `contactId` is not found, I should receive a `404 Not Found` response.
- If the request body is invalid or missing, I should receive a `400 Bad Request` response.

---

### **Scenario 4: Delete a Contact**

**Given**  
I am an authenticated user with a valid `UserId` and a valid `contactId`.

**When**  
I send a `DELETE` request to `/contact/{contactId}`.

**Then**  
- I should receive a `204 No Content` response to confirm successful deletion.
- If the `contactId` is not found, I should receive a `404 Not Found` response.
- If the request is invalid, I should receive a `400 Bad Request` response.

---

## Technical Notes

- The system must extract the `UserId` from the authentication token provided in the request.