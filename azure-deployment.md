# Azure Deployment Guide

## Introduction

This guide will walk you through the deployment of a web application to Azure. The deployment workflow in this guide is:

- Automatically deploy after every pull request is merged to development branch.
- Deploy from the development branch to a staging website.
- Review the staging site to ensure everything works correctly.
- Manually promote staging to production when we’re satisfied with the review

## Create the App Service

An App Service is Microsoft Azure’s platform-as-a-service ([PaaS](https://en.wikipedia.org/wiki/Platform_as_a_service)) offering.

1. Open [https://portal.azure.com](https://portal.azure.com) in your web browser and authenticate.
2. Navigate to App Services using the navigation menu on the left.
    - You can view all of your App Services from this screen.
3. Click the Add button.
    - A list of environments that can be hosted in Azure are displayed.
    - Clicking the different environments provides details about each option.
4. Select Web App.
5. Click Create button.
6. Add details about your application.
    - App Name - becomes part of the URL for the site.
    - Subscription - is the Azure plan that you want to use for hosting the web app.
    - Resource Group is a collection of resources that share the same permissions, lifecycle and policy. In a UI with API environment both the UI and the API would be deployed in the same Resource Group.
    - App Service plan/location is the geo location where you want your app to be hosted.
    - Application Insights can be attached directly from Azure but that is beyond the scope of this guide.
7. Click Create to build your App Service.

The Resource Group and App Service will be created. You can check the progress in the notifications section of Azure indicated by a bell icon. The notification will change to succeeded when the operation is complete.

Azure creates a placeholder website that you can view at [http://YourAppName.azurewebsites.net](http://YourAppName.azurewebsites.net).

## Configure the App Service

Microsoft provides the ability to customize Application Settings directly from Azure. Application Settings override environment variables. They become Environment Variables in most apps and Configuration Settings in Microsoft apps such as MVC and ASP.NET Core. Settings can be changed and updated at run-time, if your application supports it.

1. Navigate to App Services and select your App Service.
2. Select Application Settings.
    - This is where you can configure the environment by specifying a .NET Framework version, choosing a 32-bit or 64-bit environment and several other options.
3. Configure your environment as desired.
4. Add any Application Settings that your app needs.
5. Click Save.

## Setup Deployment Slots

Deployment slots allow multiple deployment URLs within the same App Service. You can deploy to different slots from different branches in your code repository. Each deployment slot supports unique Application Settings so you can test your staging environment along with a staging API and coordinate the deployment of both web apps. 

In this guide our desired deployment workflow is to preview the web app in staging and then manually promote it to production. Deployments can be hot swapped to promote Staging to Production and easily reversed if there is a need to rollback a change.

1. Navigate to App Services and select your app service.
2. Click Deployment slots.
3. Click Add Slot.
    - Name the new slot "staging".
    - Choose copy the configuration settings from the existing slot, if desired.

The new slot will be deployed to a unique URL. The URL is created by appending "-staging" to the main Deployment Slot’s URL (YourAppName-staging.azurewebsites.net).

## Deployment

Each slot can have a unique deployment source. If you had a branch in your code repository dedicated to staging, you could deploy directly from that branch to the staging deployment slot. In this guide we will deploy the development branch directly to the staging deployment slot.

1. Select Resource Groups.
2. Click your Resource Group.
3. Click Staging.
4. Click Deployment Options.
5. Choose Source.
    - Azure supports many deployment sources. This guide was written using GitHub as the source code repository.
6. Enter your credentials for the selected deployment source.
7. If you belong to multiple GitHub organizations, you can specify which to use.
    - Note that you must have Admin access to the GitHub repository that you’re deploying from. This is because Azure will create a service hook in GitHub to notify Azure when your source branch is updated. 
8. Choose a project from your selected repository.
9. Choose branch and select your development branch.
    - This could be any branch but this guide assumes deployment from development.
10. Performance Test – You can setup performance testing under load here but that’s beyond the scope of this guide.
11. Click Ok and the deployment will begin.

You can monitor the progress using the notifications sections (bell icon in top right) or by clicking Deployment Options again. Deployment Options will also display a list of your previous deployments.

Once your deployment is complete you can view your published site by navigating to [http://YourAppName-staging.azurewebsites.net](http://YourAppName-staging.azurewebsites.net). For now, you should still see the Azure placeholder site at [http://YourAppName.azurewebsites.net](http://YourAppName.azurewebsites.net). We will be swapping those sites in the next section.

## Swapping Deployment Slots

After you’ve reviewed your staging site and determined that it is ready for production it’s time to swap deployment slots.

1. Navigate to Deployment slots from either the staging or production App Service.
2. Click the Swap button.
3. Swap Type – Choose "Swap".
	- Alternatively, you can "Swap with Preview". Swap with preview swaps only the configuration settings at first. That allows you to preview the existing app running with new configuration settings. This is useful in situations where app settings may point to a staging API but you want to test your web app against a production API before completing the swap. 
    - Swap with preview also requires an additional step to approve and complete the swap after the preview.
4. Source and destination are self-explanatory.
    - We are swapping from staging to production.
    - If you had more deployment slots you could swap between any of them from here.
5. Warnings 
    - Shows any configuration changes that will be made when swapping the slots. It is possible to have different Application settings for different slots.
    - Any other possible issues detected by Azure.
6. Click OK to complete the swap.
7. Observe the Production and Staging sites have switched.

## Testing in Production

Another cool Azure feature is the ability to direct some traffic to the staging site with the majority still going to the production slot. This is commonly referred to as testing in production and setting this up in Azure is extremely simple.

1. Select your App Service.
2. Select Testing in production.
3. Enter a percentage into any of your deployment slots and traffic will be redirected accordingly from your production slot.

## Kudu

Kudu is the engine behind Git deployments, WebJobs and various other features in Azure Web Sites. Azure provides a Kudu portal with every deployment. You can access kudu by navigating to [https://YourAppName.scm.azurewebsites.net](https://YourAppName.scm.azurewebsites.net). From there you can verify app settings, access the developer console and lots of other helpful tools.

For more information see the Kudu project on GitHub: [http://www.github.com/projectkudu/kudu](http://www.github.com/projectkudu/kudu).