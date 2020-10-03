Setup Prereqs:

- .NET Core CLI: https://dotnet.microsoft.com/download/dotnet-core/3.1
- dotnet tool install --global dotnet-ef

# Requirements

The MiniCrm maintains a database of customer information.

- Name (Full name only)
- Email Address
- Phone Number + Extension
- Address (Line 1, Line 2, City, State, Postal Code)

## Storing Customers

MiniCrm is flexible with what information is needed in order to store a Customer, since it isn't uncommon to only have an email address or phone number initially.

- At least one major piece of information about the customer is needed: Name, Phone Number, or Email Address.
- If any Address information is provided, a full address (line 1, city, state, and postal code) are required.

## Customer Search

- MiniCrm provides basic search functionality for customers by name or email address.  Both searches will return partial results in addition to full matches.