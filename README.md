# CMHTest
Childrens Mercy Hospital Test Repo

# Build
.NET Core From a command line at the root of the project:
> dotnet build

# Deploy
Docker - From a command line
> docker build --rm --pull -f "[Path to Local Repo]\CMHTest/Dockerfile" --label "[label]" -t "cmhtest:latest" "[Path to Local Repo]\CMHTest"
> docker run --rm -d  -p 5000:5000/tcp cmhtest:latest

# Test
Integration testing done with PostMan
> POST Request URL: http://localhost:5000/api/CheckInterviews
> Body: {"dateOfInterview":"[Date]"}

## Recommended
Visual Studio Code - Extensions: .Net Core, Docker