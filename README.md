# Set Up

## Prerequisite
- Have Visual Studio installed
- Have this repo on your local machine

## Running the project
- Open the project file CytidelChallenge.sln in Visual Studio
- Once it loads, right click the solution and click `Configure Startup Projects` from the dropdown
- Select the `Multiple startup projects` radio button.
- Set the Actions for both the client and the server to `Start`

![image](https://github.com/user-attachments/assets/29960041-c1f3-4871-ba12-839a46240f9a)

Alternatively, you can publish the solution by right clicking CytidelChallenge.Server, choosing the folder option, then clicking publish.This will create a Release version of the app, which will be available in the \bin\release\net8.0\publish directory of the repo. Run the app by double clicking the CytidelChallenge.Server.exe and navigating to http://localhost:5000

## Sign in details
The app stores user logins with ASP.NET Identity SQLite tables. There is one user which gets seeded on set up:

`Username: testing`
`Pwd: tEsting1234?`

More users can be seeded by updating the Program.cs
