# Asp.net Core MVC project & Asp.net Core WebApi project
This porject have 2 part, the first one is Asp.net Core MVC project, which is focus on develop a Web application for bank. The other one is a Asp.net Core WebApi project. This web application is for manage the bank application. Run on .Net Core 6 and using Entity Framework Core to access the MySQL Database.

### Logical architecture diagram of porject 
![architecture diagram ](https://firebasestorage.googleapis.com/v0/b/whitegive-bc20c.appspot.com/o/images%2FLogical%20diagram.png?alt=media&token=061b7f24-74c0-4dde-b188-c63b09b15546)

### Features:
* Asp.net Core MVC project(Bank web application)
  * Deposit
   <br>User able to use deposit function in their personal account when they are login. 
  * Withdraw
   <br>User able to use withdraw function in their personal account when they are login.
  * Transfer 
   <br>User able to use transfer function to transfer money between different account when they are login.
  * My Statement
    <br>User able to view their statement including see the current balance of a specific account and view all the account’s transactions when they are login.
  * My Profile 
    <br>My Profile page allows the user to modify customer information, permitting the editing of Name, TFN, Address, Suburb, State, Postcode and Mobile.
  * BillPay 
    <br>Page shows the user their currently scheduled bills to be paid and includes a link to create a new entry. Additionally,there is an option to modify a scheduled bill, including the ability to cancel a payment
 * Asp.net Core WebApi project(Admin web application)
  * View transaction history
   <br>View transaction history for an account. Web page able to generate a table displaying the transaction history for an account within a specified start date and end date period 
  * Modify user's profile
   <br>admin able to Modify a customer’s profile details Name, TFN, Address, Suburb, State, Postcode and Mobile.
  * Lock and unlock a customer’s login 
   <br>Once locked the customer should not be able to login to the Customer Website until unlocked by an admin.
  * Block and unblock scheduled payments
    <br>Blocked bills can displayed as “Blocked” within the BillPay list on the Customer Website. The customer cannot unblock the payment, although the customer can delete the bill if desired. When a scheduled payment is blocked it should not run until unblocked by an admin.
