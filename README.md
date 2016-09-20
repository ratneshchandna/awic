# AWIC v1.0.0

AWIC is a website (a lightweight web application to be accurate) for AWIC Community and Social Services, a non profit that, among other things, provides settlement and employment services as well as training and volunteer opportunities for newcomers to Canada. 

**Important Notice**: This web application has been designed and developed by [Ratnesh Chandna](https://github.com/ratneshchandna) as a freelance project for AWIC Community and Social Services. As such, **it is not intended to be copied and published/distributed (modified or unmodified) freely** and the following copyright notice applies: 

*Copyright © 2015 by AWIC All rights reserved. This website and the code written to create this website, or any portion thereof, may not be reproduced and published/distributed (modified or unmodified) in any manner whatsoever without the express written permission of the website developer.*

With the above copyright notice being laid out, anyone viewing this repository, however, has full right to clone it and view the code within it for learning purposes. If the viewer is interested in making the application better, he/she may fork the repository, make the necessary changes and submit a pull request. If the pull request is approved, the viewer's name will be added to the "Team" section of this readme as a helpful contributor. It should be obvious that no change to the readme will be accepted in any pull request. 

## Technologies and Frameworks Used

* ASP.NET MVC 5
* EF 6 Code First
* ASP.NET Identity 2

## Libraries Used

* Stripe (for making online donations and paying membership fees)

## Running the Application

1. Open the solution in Visual Studio 2013. Build the solution to install Nuget packages. This step will automatically restore Nuget packages. Please ensure you have the latest version of Nuget installed. 
2. Once the application is running, HTTP REST calls to Stripe and connection attempts to the SMTP server will not work, as their settings (API Keys, usernames, passwords, etc.) are not checked into this repository for security purposes. The settings, however, can be conveniently configured in the config files checked into this repository. You can find them under the "AWIC" folder as "AppSettings.config" and "WebConnectionStrings.config". These files already have a template which just needs to be filled in with your own API Keys, usernames, passwords, etc. 
3. If you do not wish to change these settings right away, the application will work to the extent of accessing the DB only (as the default web.config file uses LocalDB), without being able to use Stripe or email functionality. If you decide to publish the application to a remote server (again, for learning purposes only), the connection strings will change to those in "WebConnectionStrings.config" and you'll have to change that file before publishing to able to access your DB. 

## Team

* [Ratnesh Chandna](https://github.com/ratneshchandna) - Lead Designer and Developer
