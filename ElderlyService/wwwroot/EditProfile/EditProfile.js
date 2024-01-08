function validateForm() {
    var fname = document.getElementById("fName").value;
    var lname = document.getElementById("lName").value;
    var email = document.getElementById("Email").value;
    var phoneNumber = document.getElementById("Phone").value;
    var dateOfBirth = document.getElementById("date").value;
    var priceOfService = document.getElementById("PriceOfService").value;

    var nameError = document.getElementById("nameError");
    var emailError = document.getElementById("emailError");
    var phoneError = document.getElementById("phoneError");
    var dateError = document.getElementById("dateError");
    var priceOfServiceError = document.getElementById("PriceOfServiceError");

    // Reset previous errors
    nameError.innerHTML = "";
    emailError.innerHTML = "";
    phoneError.innerHTML = "";
    dateError.innerHTML = "";
    priceOfServiceError.innerHTML = "";

    var isValid = true;

    // Validate Name
    if (fname.trim() === "" || lname.trim() === "") {
        nameError.innerHTML = "First and Last Name are required";
        isValid = false;
    }

    // Validate Email
    if (!isValidEmail(email)) {
        emailError.innerHTML = "Invalid email address";
        isValid = false;
    }

    // Validate Phone Number
    if (phoneNumber.trim() === "") {
        phoneError.innerHTML = "Phone number is required";
        isValid = false;
    } else if (!isValidPhoneNumber(phoneNumber)) {
        phoneError.innerHTML = 'Invalid phone number format (e.g., 1234567890)';
        isValid = false;
    }

    // Validate Date of Birth (minimum 22 years)
    if (dateOfBirth.trim() === "") {
        dateError.innerHTML = "Date of birth is required";
        isValid = false;
    } else {
        var currentDate = new Date();
        var selectedDate = new Date(dateOfBirth);
        var ageDifference = currentDate.getFullYear() - selectedDate.getFullYear();

        if (ageDifference < 22) {
            dateError.innerHTML = "Must be at least 22 years old";
            isValid = false;
        }
    }

    // Validate Price of Service (non-negative)
    if (parseFloat(priceOfService) < 0) {
        priceOfServiceError.innerHTML = "Price cannot be negative";
        isValid = false;
    }

    return isValid;
}
