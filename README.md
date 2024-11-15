# Ambassador Companion
“Ambassador Companion” is an app for **Windows 10** and **Xbox One** (and now **Xbox Series X/S**) designed to bring the full experience of the [Ambassadors website](https://ambassadors.microsoft.com/xbox) to the console and desktop.

# Note to the community
As many of you may already know, Microsoft has discontinued the Xbox Ambassadors program. During the many years I was part of the community, I had the immense pleasure of experiencing incredible things, which allowed me to see that gaming should always be a safe and inclusive environment for everyone.

Although not in an official capacity, the development of an app for Xbox Ambassadors was my way of listening to the many ambassadors who requested such an app and, at the same time, making a small contribution to this very special community.

I am immensely grateful to everyone who supported me during the development of this app, as well as to those who gave me their support after its release. I sincerely apologize to anyone who experienced technical difficulties or encountered any bugs while using it.

This repository will now be archived, but feel free to use the code in other projects or simply as a learning tool.

Thank you.

## Features
During the development of this project, we tried to bring all the features of the [ambassadors platform](https://ambassadors.microsoft.com/xbox) in one place. Our application, initially had the following features:
* ~~Responder / Twitter~~ *(Discontinued)*
* ~~Leaderboard~~ *(Discontinued)*
* Ambassadors home page
  * Profile info
  * Ranking
  * Latest Blog News
  * Ambassador of the month
  * Latest missions
  * Latest Xbox forum posts
* Profile page
  * Profile info containing performance charts
  * Badges & Pins
  * Specializations
  * Activities
  * Reactions
* Missions page
  * Active missions
  * Completed missions
  * Self report missions
* Integrated Xbox forum
* Sweepstakes page
* ~~Video gallery integrated with YouTube~~ *(Discontinued)*
* Rewards page (Read only)
* ~~Accademy~~ *(Discontinued)*
  * ~~Quizzes~~
  * ~~Courses~~
* Ambassador Handbook
* Xbox Live Status page
* Notifications page
* ~~Responder Notifications~~
* Blog Notifications

## Running the project
To run the project it is necessary to install [Visual Studio Community 2019](https://visualstudio.microsoft.com/). Remember to check the option "Universal Windows Platform - UWP" during the installation.

### Running the video Gallery
To run the video gallery, you need to **obtain a key** from the [YouTube data api](https://developers.google.com/youtube/v3). It must be added to the **App.xaml** file.

> <y:Key x:Key="api_key" PublicKey="**Your Public Key**" SecretKey="no_needed"/>

## Contributing to the project
Feel free to send PRs. But before sending a new code, open an issue to talk about your idea.

No code that attempts to cheat or violate Xbox Ambassadors rules will be accepted.

No code that collects personal data that can identify an ambassador will be accepted. The privacy and security of each ambassador must be respected. Only anonymous telemetry data about the use of the app will be collected.

We are looking forward to your contributions 😉 !!

## Disclaimer
MICROSOFT, XBOX, and XBOX AMBASSADORS BRANDS NAME AND LOGO ARE PROPERTY OF MICROSOFT CORPORATION. ALL RIGHTS RESERVED. MICROSOFT HAS NOT DEVELOPED AND IS NOT RESPONSIBLE FOR THIS APPLICATION.
