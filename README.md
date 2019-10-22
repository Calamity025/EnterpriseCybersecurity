# How to run

To run this project you need: 
- .Net Core SDK 2.2;
- Node.js 10.x.x;

.Net Core - https://dotnet.microsoft.com/download/dotnet-core
Node.js - https://nodejs.org/download/release/v10.15.1/

Open the solution in visual studio and right click on solution in Solution Explorer, choose restore NuGet packages if available. Next right click on solution again and press rebuild, when build is finished choose startup profile IIS Express as shown below and press it.  

![alt text](https://i.imgur.com/dU1CXEt.png)

After installing Node run in node.js command prompt `cd <PATH _TO _PROJECT_FOLDER>\Frontpage; npm install` and wait for completion. Then run in the same directory `ng serve`


