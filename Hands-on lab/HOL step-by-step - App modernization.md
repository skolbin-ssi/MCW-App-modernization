![Microsoft Cloud Workshops](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")

<div class="MCWHeader1">
App modernization
</div>

<div class="MCWHeader2">
Hands-on lab step-by-step guide
</div>

<div class="MCWHeader3">
April 2021
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2021 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [App modernization hands-on lab step-by-step](#app-modernization-hands-on-lab-step-by-step)
  - [Abstract and learning objectives](#abstract-and-learning-objectives)
  - [Overview](#overview)
  - [Solution architecture](#solution-architecture)
  - [Requirements](#requirements)
  - [Exercise 1: Setting up Azure Migrate](#exercise-1-setting-up-azure-migrate)
  - [Exercise 2: Migrate your application with App Service Migration Assistant](#exercise-2-migrate-your-application-with-app-service-migration-assistant)
    - [Task 1: Perform assessment for migration to Azure App Service](#task-1-perform-assessment-for-migration-to-azure-app-service)
    - [Task 2: Migrate the web application to Azure App Service](#task-2-migrate-the-web-application-to-azure-app-service)
  - [Exercise 3: Migrate the on-premises database to Azure SQL Database](#exercise-3-migrate-the-on-premises-database-to-azure-sql-database)
    - [Task 1: Perform assessment for migration to Azure SQL Database](#task-1-perform-assessment-for-migration-to-azure-sql-database)
    - [Task 2: Retrieve connection information for SQL Databases](#task-2-retrieve-connection-information-for-sql-databases)
    - [Task 3: Migrate the database schema using the Data Migration Assistant](#task-3-migrate-the-database-schema-using-the-data-migration-assistant)
    - [Task 4: Migrate the database using the Azure Database Migration Service](#task-4-migrate-the-database-using-the-azure-database-migration-service)
    - [Task 5: Configure the application connection to SQL Azure Database](#task-5-configure-the-application-connection-to-sql-azure-database)
  - [Exercise 4: Setup CI/CD pipeline with GitHub Actions for the web app](#exercise-4-setup-cicd-pipeline-with-github-actions-for-the-web-app)
    - [Task 1: Moving the codebase to a GitHub repo](#task-1-moving-the-codebase-to-a-github-repo)
    - [Task 2: Creating a staging deployment slot](#task-2-creating-a-staging-deployment-slot)
    - [Task 3: Setting up CI/CD with GitHub Actions](#task-3-setting-up-cicd-with-github-actions)
    - [Task 4: Pushing code changes to staging and production](#task-4-pushing-code-changes-to-staging-and-production)
    - [Task 5: Swap deployment slots to move changes in staging to production](#task-5-swap-deployment-slots-to-move-changes-in-staging-to-production)
  - [Exercise 5: Using serverless Azure Functions to process orders](#exercise-5-using-serverless-azure-functions-to-process-orders)
    - [Task 1: Deploying Azure Functions](#task-1-deploying-azure-functions)
    - [Task 2: Connecting Function App and App Service](#task-2-connecting-function-app-and-app-service)
    - [Task 3: Testing serverless order processing](#task-3-testing-serverless-order-processing)
    - [Task 4: Enable Application Insights on the Function App](#task-4-enable-application-insights-on-the-function-app)
  - [After the hands-on lab](#after-the-hands-on-lab)
    - [Task 1: Delete Azure resource groups](#task-1-delete-azure-resource-groups)
    - [Task 2: Delete GitHub repository](#task-2-delete-github-repository)
    - [Task 3: Remove GitHub Authorized Apps](#task-3-remove-github-authorized-apps)

<!-- /TOC -->

# App modernization hands-on lab step-by-step

## Abstract and learning objectives

In this hands-on lab, you implement the steps to modernize a legacy on-premises application, including upgrading and migrating the application and the database to Azure and updating the application to take advantage of serverless and cloud services.

At the end of this hands-on lab, your ability to build solutions for modernizing legacy on-premises applications and infrastructure using cloud services will be improved.

## Overview

Parts Unlimited is an online auto parts store. Founded in Spokane, WA, in 2008, they are providing both genuine OEM and aftermarket parts for cars, sport utility vehicles, vans, and trucks, including new and remanufactured complex parts, maintenance items, and accessories. Its mission is to make buying vehicle replacement parts easy for consumers and professionals. Parts Unlimited has 185 stores in the US, with plans to scale to Mexico and Brazil.

Parts Unlimited has a hosted web application on its internal infrastructure and using a Windows Server, Internet Information Services (IIS), and Microsoft SQL Server to host the solution. Beyond the initial effort and costs, these applications incur ongoing maintenance costs in hardware, operating system updates, and licensing fees. These maintenance costs make Microsoft Azure App Service an attractive alternative. Their team is looking to migrate Microsoft ASP.NET applications and any SQL Server database to Azure App Service and Azure SQL Database. However, they are worried that their application might not be supported. Their website is built on a .NET Core version that hit the end of life on December 23, 2019. They wonder if they can move to the cloud now and migrate their application later or if the old version will be a show stopper.

Additionally, Parts Unlimited has plans to increase its marketing investment, currently on hold because of scaling issues. The company is stuck and can't grow without increasing its infrastructure footprint. Their CEO wants to finalize their cloud vs. on-premises decision based on the current migration effort's success. The engineering team is worried about their order processing subsystem. Currently, they have a strongly coupled order processing system that runs synchronously during checkout. When moved to the cloud, they don't want to be worried about their order processing system's scalability. They are looking for a modern approach with the least migration effort possible.

Finally, Parts Unlimited is looking to invest in DevOps practices to decrease human error in deployments. They are looking for options to have a staging environment to test functionality before shipping to production. However, their team does not have any experience in building CI/CD pipelines. They are not sure if this goal is achievable in the short term, and they do not want it to hold up their migration to the cloud.

## Solution architecture

Below is a high-level architecture diagram of the solution you implement in this hands-on lab. Please review this carefully to understand the whole of the solution as you are working on the various components.

![This solution diagram includes a high-level overview of the architecture implemented within this hands-on lab.](media/architecture-diagram.png "Solution architecture diagram")

> **Note:** The solution provided is only one of many possible, viable approaches.

The solution begins with setting up Azure Migrate as the central assessment and migration hub for Parts Unlimited's E-Commerce website. Using the App Service Migration Assistant tool from Azure Migrate, Parts Unlimited found that their website is fully compatible with Azure App Service. As a next step, they use App Service Migration Assistant to provision an Azure App Service environment and deploy their application to Azure. Following the success in moving the web application, Parts Unlimited uses the Data Migration Assistant (DMA) assessment to determine that they can migrate into a fully-managed SQL Database service in Azure. The assessment reveals no compatibility issues or unsupported features that would prevent them from using Azure SQL Database.

Next, Parts Unlimited sets up a private GitHub repository and pushes their codebase to GitHub. They set up deployment slots to have a staging environment to test functionality before releasing to production. As a CI/CD solution, they decide to use GitHub Actions and Workflows.

Finally, Parts Unlimited decides to decouple its order processing system and move to an event-driven serverless compute platform. Following a [web-queue-worker architecture](https://docs.microsoft.com/en-us/azure/architecture/guide/architecture-styles/web-queue-worker), they build an Azure Function and use Azure Storage Queue to process orders and create invoices asynchronously. When new orders come in, the web front end adds jobs into the queue consumed by Azure Functions. The Functions App scales independently based on the number of jobs in the queue, helping Parts Unlimited elastically handle a variable amount of orders.

## Requirements

- Microsoft Azure subscription must be pay-as-you-go or MSDN.
  - Trial subscriptions will _not_ work.
  - **IMPORTANT:** To complete this lab, you must have sufficient rights within your Azure AD tenant to register resource providers in your Azure Subscription.
- An active GitHub Account.
  
## Exercise 1: Setting up Azure Migrate

Duration: 10 minutes

Azure Migrate provides a centralized hub to assess and migrate on-premises servers, infrastructure, applications, and data to Azure. It provides a single portal to start, run, and track your migration to Azure. Azure Migrate comes with a range of assessment tools and migration that we will use during our lab. We will use Azure Migrate as the central location for our assessment and migration efforts.

1. In the [Azure portal](https://portal.azure.com), navigate to your lab resource group. Select **Add** to add a new resource.

    ![Lab resource group is open. Resource Add button is highlighted.](media/portal-add-resource.png "Lab Resource Group")

2. Type **Azure Migrate** into the search box and hit **Enter** to start the search.

    ![Azure Portal new resource page is open. The search box is filled with Azure Migrate.](media/azure-migrate-search.png "Marketplace Search for Azure Migrate")

3. Select **Create** to continue.

    ![Azure Migrate resource creation screen is open. Create button is highlighted.](media/azure-migrate-create.png "Creating Azure Migrate")

4. As part of our migration project for Parts Unlimited, we will first assess and migrate their Web Application living on IIS, on a VM. Select **Web Apps** to continue.

    ![Azure Migrate is open. Web Apps section is highlighted.](media/azure-migrate-web-apps.png "Azure Migrate Web Apps")

5. Select **Create project**.

    ![Azure Migrate is open. Web Apps section is selected. Create project button is highlighted.](media/azure-migrate-create-project.png "Azure Migrate Create project")

6. Type **partsunlimitedweb** as your project name. Select **Create** to continue.

    ![Azure Migrate project settings page is shown. The project name is set to partsunlimitedweb. Create button is highlighted.](media/azure-migrate-create-project-settings.png "Azure Migrate Project Creation")

7. Once your project is created **Azure Migrate** will show you default **Web App Assessment** and **Web App Migration** tools (You might need to refresh your browser). For the Parts Unlimited web site, **App Service Migration Assistant** is the one we have to use. Download links are presented on Azure Migrate's Web Apps page. In our case, our lab environment comes with App Service Migration Assistant pre-installed on Parts Unlimited's web server.

    ![Azure Migrate Web App assessment and migration tools are presented.](media/azure-migrate-web-app-migration.png "Azure Migrate Web Apps Capabilities")

8. Another aspect of our migration project will be the database for Parts Unlimited's website. We will have to assess the database's compatibility and migrate to Azure SQL Database. Let's switch to the **SQL Server (only)** section in Azure Migrate. Select **Click here** hyperlink for Assessment tools.

    ![Azure Migrate is open. The databases section is selected. Click here link for assessment tools is highlighted.](media/azure-migrate-database-assessment.png "Azure Migrate Databases")

9. We will use **Azure Migrate: Database Assessment** to assess Parts Unlimited's database hosted on a SQL Server 2008 R2 server. Pick **Azure Migrate: Database Assessment (1)** and select **Add tool (2)**.

    ![Azure Migrate Database Assessment option is selected for Azure Migrate tools. Add tool button is highlighted.](media/azure-migrate-database-assessment-tool.png "Azure Migrate Database Assessment Tools")

10. Now, we can see a download link for the **Data Migration Assessment (1)** tool under assessment tools in Azure Migrate. In our case, our lab environment comes with the Data Migration Assessment pre-installed on Parts Unlimited's database server. Select **Click here (2)** under the **Migration Tools** section to continue.

    ![Data Migration Assessment tool's download link is shown. Click here link for migration tools is highlighted.](media/azure-migrate-database-migration.png "Azure Migrate DMA Download")

11. We will use **Azure Migrate: Database Migration** to migrate Parts Unlimited's database to an Azure SQL Database. Pick **Azure Migrate: Database Assessment (1)** and select **Add tool (2)**.

    ![Azure Migrate Database Migration option is selected for Azure Migrate tools. Add tool button is highlighted.](media/azure-migrate-database-migration-tool.png "Azure Migrate Database Migration Tool")

12. Now, we have all the assessment and migration tools/services we need for Parts Unlimited ready to go under the Azure Migrate umbrella.

    ![Azure Migrate databases section is open. Azure Migrate Database Assessment and Database Migration tools are presented.](media/azure-migrate-database-migration-ready.png "Azure Migrate Database Migration and Assessment Tools")

## Exercise 2: Migrate your application with App Service Migration Assistant

Duration: 20 minutes

The first step for Parts Unlimited is to assess whether their apps have dependencies on unsupported features on Azure App Service. In this exercise, you use an **Azure Migrate** tool called the [App Service migration assistant](https://appmigration.microsoft.com/) to evaluate Parts Unlimited's website for a migration to Azure App Service. The assessment runs readiness checks and provides potential remediation steps for common issues. Once the assessment succeeds, we will proceed with the migration as well. You will use a simulated on-premises environment hosted in virtual machines running on Azure.

### Task 1: Perform assessment for migration to Azure App Service

Parts Unlimited would like an assessment to see what potential issues they might need to address in moving their application to Azure App Service. You will use the [App Service migration assistant](https://appmigration.microsoft.com/) to assess the application and run various readiness checks.

1. In the [Azure portal](https://portal.azure.com), navigate to your **WebVM** VM by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **WebVM** VM from the list of resources.

    ![The WebVM virtual machine is highlighted in the list of resources.](media/webvm-selection.png "WebVM Selection")

2. On the WebVM Virtual Machine's **Overview (1)** blade, copy the **Public IP address (2)**.

    ![The WebVM VM blade is displayed, Public IP Address copy button is highlighted.](media/web-vm-ip.png "WebVM Overview and Public IP")

3. Open a new browser window and navigate to the IP Address you copied.

    ![The WebVM VM blade is displayed, Public IP Address copy button is highlighted.](media/parts-umlimited-web-site.png "Parts Unlimited Web Site")

    > For testing purposes, you might want to create yourself an account on the Parts Unlimited website and purchase some products. Use the coupon code **FREE** to buy everything for free.

4. Go back to the Azure Portal. On the WebVM Virtual Machine's **Overview** blade, select **Connect (1)** and **RDP (2)** on the top menu.

   ![The WebVM VM blade is displayed, with the Connect button highlighted in the top menu.](media/connect-rdp-webvm.png "WebVM RDP Connect")

5. Select **Download RDP File** on the next page, and open the downloaded file.

    > **Note**: The first time you connect to the WebVM Virtual Machine, you will see a blue pop-up terminal dialog taking you through a couple of software installs. Don't be alarmed, and wait until the installs are complete.

    ![RDP Window is open. Download RDP File button is highlighted.](media/rdp-download.png "WebVM RDP File Download")

6. Select **Connect** on the Remote Desktop Connection dialog.

    ![In the Remote Desktop Connection Dialog Box, the Connect button is highlighted.](media/remote-desktop-connection-webvm.png "Remote Desktop Connection dialog")

7. Enter the following credentials with your password when prompted, and then select **OK**:

   - **Username**: demouser
   - **Password**: {YOUR-ADMIN-PASSWORD}
  
    > **Note**: default password is `Password.1!!`

    ![The credentials specified above are entered into the Enter your credentials dialog.](media/rdp-credentials-webvm.png "Enter your credentials")

8. Select **Yes** to connect, if prompted that the identity of the remote computer cannot be verified.

    ![In the Remote Desktop Connection dialog box, a warning states that the remote computer's identity cannot be verified and asks if you want to continue anyway. At the bottom, the Yes button is circled.](media/remote-desktop-connection-identity-verification-webvm.png "Remote Desktop Connection dialog")

9. Once logged into the WebVM VM, a script will execute to install the various items needed for the remaining lab steps.
10. Once the script completes, open **AppServiceMigrationAssistant** that is located on the desktop.

    ![AppServiceMigrationAssistant is highlighted on the desktop.](media/appservicemigrationassistant-desktop.png "App Service Migration Assistant")

11. Once App Service Migration Assistant discovers the websites available on the server, choose **Default Web Site (1)** for migration and select **Next (2)** to start the assessment.

    ![AppServiceMigrationAssistant is open. Default Web Site is selected. The next button is highlighted.](media/appservicemigration-choose-site.png "App Service Migration Assistant Web Site selection")

12. Observe the result of the assessment report. In our case, our application has successfully passed 13 tests **(1)** with no additional actions needed. Now that our assessment is complete, select **Next (2)** to proceed to the migration.

   ![Assessment report result is shown. There are 13 success metrics presented. The next button is highlighted.](media/appservicemigration-report.png "Assessment Report")

   > For the details of the readiness checks, see [App Service Migration Assistant documentation](https://github.com/Azure/App-Service-Migration-Assistant/wiki/Readiness-Checks).

### Task 2: Migrate the web application to Azure App Service

After reviewing the assessment results, you have ensured the web application is a good candidate for migration to Azure App Service. Now, we will continue the process with the migration of the application.

1. In order to continue with the migration of our website Azure App Service Migration Assistant needs access to our Azure Subscription. Select **Copy Code & Open Browser** button to be redirected to the Azure Portal.

   ![Azure App Service Migration Assistant's Azure Login dialog is open. A device code is presented. Copy Code & Open Browser button is highlighted.](media/appservicemigration-azure-login.png "Azure Login")

2. At its first launch, you will be asked to choose a browser. Select **Microsoft Edge (1)**  and check **Always use this app (2)** checkbox. Select **OK (2)** to move to the next step.

    ![Browser choice dialog is shown. Microsoft Edge is selected. Always use this app checkbox is checked. OK button is highlighted.](media/browser-choice.png "Default Browser Selection")

3. Right-click the text box and select **Paste (1)** to paste your login code. Select **Next** to give subscription access to App Service Migration Assistant.

    ![Azure Code Login website is open. Context menu for the code textbox is shown. Paste command from the context menu is highlighted. The next button is highlighted as a second step. ](media/appservicemigration-azure-login-code.png "Enter Authentication Code")

4. Continue the login process with your Azure Subscription credentials. When you see the message that says **You have signed in to the Azure App Service Migration Assistant application on your device** close the browser window and return to the App Service Migration Assistant.

    ![Azure Login process is complete. A message dialog is shown that indicates the login process is a success.](media/appservicemigration-azure-login-complete.png "App Service Migration Assistant authentication approval")

5. Select the Azure Migrate project we created **(1)** in the previous exercise to submit our migration results. Select **Next** to continue.

    ![Azure Migrate Project is set to partsunlimitedweb. The next button is highlighted.](media/appservicemigration-azure-migrate.png "Azure Migrate Hub integration")

6. In order to migrate Parts Unlimited website, we have to create an App Service Plan. The Azure App Service Migration Assistant will take care of all the requirements needed. Select **Use existing (1)** and select the lab resource group as your deployment target. App Service requires a globally unique Site Name. We suggest using a pattern that matches `partsunlimited-web-{uniquesuffix}` **(2)**. Select **Migrate** to start the migration process.

    ![Deployment options are presented. The existing lab resource group is selected as the destination. The destination site name is set to partsunlimited-web-20X21. Migrate button is highlighted.](media/appservicemigration-migrate.png "Azure App Service Migration Assistant Options")

    > **WARNING:** If your migration fails with a **WindowsWorkersNotAllowedInLinuxResourceGroup (1)** try the migration process again, but this time selecting a different Resource Group for your deployment. If that is not possible, select a different Region.  
    >
    > ![Migration failed error screen is shown. WindowsWorkersNotAllowedInLinuxResourceGroup message is highlighted.](media/app-migration-windowsworkersnotallowed.png "Migration failed")

7. We have just completed the Parts Unlimited website's migration from IIS on a Virtual Machine to Azure App Service. Congratulations. Let's go back to the Azure Portal and look into Azure Migrate. Search for `migrate` **(1)** on the Azure Portal and select **Azure Migrate (2)**.

    ![Azure Portal is open. The search box is filled with the migrate keyword. Azure Migrate is highlighted from the result list.](media/find-azure-migrate.png "Azure Migrate on Azure Portal Search")

8. Switch to the **Web Apps (1)** section. See the number of discovered web servers, assessed websites **(2)** and migrated websites change **(3)**. Keep in mind that you might need to wait for 5 to 10 minutes for results to show up. You can use the **Refresh** button on the page to see the latest status.

    ![Azure Migrate shows web app assessment and migration reports.](media/azure-migrate-web-app-migration-done.png "Azure Migrate Web Apps Tools")

## Exercise 3: Migrate the on-premises database to Azure SQL Database

Duration: 55 minutes

The next step of Part Unlimited's migration project is the assessment and migration of its database. Currently, the database lives on a SQL Server 2008 R2 on a virtual machine. You will use an **Azure Migrate: Database Assessment** tool called **Microsoft Data Migration Assistant (DMA)** to assess the `PartsUnlimited` database for a migration to Azure SQL Database. The assessment generates a report detailing any feature parity and compatibility issues between the on-premises database and Azure SQL Database. After the assessment, you will use an **Azure Migrate: Database Migration** service called **Azure Database Migration Service (DMS)**. During the exercise, you will use a simulated on-premises environment hosted in virtual machines running on Azure.

### Task 1: Perform assessment for migration to Azure SQL Database

Parts Unlimited would like an assessment to see what potential issues they might need to address in moving their database to Azure SQL Database. In this task, you will use the [Microsoft Data Migration Assistant](https://docs.microsoft.com/sql/dma/dma-overview?view=sql-server-2017) (DMA) to assess the `PartsUnlimited` database against Azure SQL Database (Azure SQL DB). Data Migration Assistant (DMA) enables you to upgrade to a modern data platform by detecting compatibility issues that can impact database functionality on your new version of SQL Server or Azure SQL Database. It recommends performance and reliability improvements for your target environment. The assessment generates a report detailing any feature parity and compatibility issues between the on-premises database and the Azure SQL DB service.

> **Note**: The Database Migration Assistant is already installed on your SqlServer2008 VM. It can be downloaded through Azure Migrate or from the [Microsoft Download Center](https://go.microsoft.com/fwlink/?linkid=2090807) as well.

1. Connect to your SqlServer2008 VM with RDP. Your credentials are the same as the WebVM , `demouser` with `Password.1!!` password.

   ![The SQLServer2008 virtual machine is highlighted in the list of resources.](media/find-sqlserver2008-resource.png "SqlServer2008 Selection")

2. Launch DMA from the Windows Start menu by typing "data migration" into the search bar and then selecting **Microsoft Data Migration Assistant** in the search results.

    > **Note**: If you do not see the migration assistant, install it from the `c:\DataMigrationAssistant.msi` file.

    > **Note**: There is a known issue with screen resolution when using an RDP connection to Windows Server 2008 R2, which may affect some users. This issue presents itself as very small, hard-to-read text on the screen. The workaround for this is to use a second monitor for the RDP display, allowing you to scale up the resolution to make the text larger.

   ![In the Windows Start menu, "data migration" is entered into the search bar, and Microsoft Data Migration Assistant is highlighted in the Windows start menu search results.](media/windows-start-menu-dma.png "Data Migration Assistant")

3. In the DMA dialog, select **+** from the left-hand menu to create a new project.

   ![The new project icon is highlighted in DMA.](media/dma-new.png "New DMA project")

4. In the New project pane, set the name of the project **(1)** and make sure the following value are selected:

   - **Project type**: Select Assessment.
   - **Project name (1)**: Enter **Assessment**.
   - **Assessment type**: Select Database Engine.
   - **Source server type**: Select SQL Server.
   - **Target server type**: Select Azure SQL Database.

   ![New project settings for doing an assessment of a migration from SQL Server to Azure SQL Database.](media/dma-new-project-to-azure-sql-db.png "New project settings")

5. Select **Create (2)**.

6. On the **Options** screen, ensure **Check database compatibility (1)** and **Check feature parity (1)** are both checked, and then select **Next (2)**.

   ![Check database compatibility and check feature parity are checked on the Options screen.](media/dma-options.png "DMA options")

7. On the **Sources** screen, select **Add sources**.
8. 
9. Enter the following into the **Connect to a server** dialog that appears on the right-hand side:

    - **Server name (1)**: Enter **SQLSERVER2008**.
    - **Authentication type (2)**: Select **SQL Server Authentication**.
    - **Username (3)**: Enter **PUWebSite**
    - **Password (4)**: Enter **{YOUR-ADMIN-PASSWORD}** `Password.1!!`
    - **Encrypt connection**: Check this box if not checked.
    - **Trust server certificate (5)**: Check this box.

    ![In the Connect to a server dialog, the values specified above are entered into the appropriate fields.](media/dma-connect-to-a-server.png "Connect to a server")

9. Select **Connect (6)**.

10. On the **Add sources** dialog that appears next, check the box for `PartsUnlimited` **(1)** and select **Add (2)**.

    ![The PartsUnlimited box is checked on the Add sources dialog.](media/dma-add-sources.png "Add sources")

11. Select **Start Assessment**.

    ![Start assessment](media/dma-start-assessment-to-azure-sql-db.png "Start assessment")

12. Take a moment to review the assessment for migrating to Azure SQL DB. The SQL Server feature parity report **(1)** shows that Analysis Services and SQL Server Reporting Services are unsupported **(2)**, but these do not affect any objects in the `PartsUnlimited` database, so they won't block a migration.

    ![The feature parity report is displayed, and the two unsupported features are highlighted.](media/dma-feature-parity-report.png "Feature parity")

13. Now, select **Compatibility issues (1)** so you can review that report as well.

    ![The Compatibility issues option is selected and highlighted.](media/dma-compatibility-issues.png "Compatibility issues")

    The DMA assessment for migrating the `PartsUnlimited` database to a target platform of Azure SQL DB reveals that no issues or features are preventing Parts Unlimited from migrating their database to Azure SQL DB.

14. Select **Upload to Azure Migrate** to upload assessment results to Azure.

    ![Upload to Azure Migrate button is highlighted.](media/dma-upload-azure-migrate.png "Azure Migrate Upload")

15. Select the right Azure environment **(1)** your subscription lives. Select **Connect (2)** to proceed to the Azure login screen.

    ![Azure is selected as the Azure Environment on the connect to Azure screen. Connect button is highlighted.](media/dma-azure-migrate-upload.png "Azure Environment Selection")

16. Select your subscription **(2)** and the `partsunlimited` Azure Migrate project **(3)**. Select **Upload (4)** to start the upload to Azure.

    ![Upload to Azure Migrate page is open. Lab subscription and partsunlimited Azure Migrate Project are selected. Upload button is highlighted.](media/dma-azure-migrate-upload-2.png "Azure Migrate upload settings")

    > **Note**: If you encounter **Failed to fetch subscription list from Azure, Strong Authentication required (1)** you might not see some of your subscriptions because of MFA limitations. You should still be able to see your lab subscription.

17. Once the upload is complete select **OK** and navigate to the Azure Migrate page on the Azure Portal.

    ![Assessment Uploaded dialog shown.](media/dma-upload-complete.png "Assessment Uploaded")

18. Select the **Databases (1)** page on Azure Migrate. Observe the number of assessed database instances **(2)** and the number of databases ready for Azure SQL DB **(2)**. Keep in mind that you might need to wait for 5 to 10 minutes for results to show up. You can use the **Refresh** button on the page to see the latest status.

    ![Azure Migrate Databases page is open. The number of assessed database instances and the number of databases ready for Azure SQL DB shows one.](media/dma-azure-migrate-web.png "Azure Migrate Database Assessment")

### Task 2: Retrieve connection information for SQL Databases

In this task, you will retrieve the IP address of the SqlServer2008 VM and the Fully Qualified Domain Name for the Azure SQL Database. This information is needed to connect to your SqlServer2008 VM and Azure SQL Database from Azure Data Migration Service and Azure Data Migration Assistant.

1. In the [Azure portal](https://portal.azure.com), navigate to your **SqlServer2008-ip** resource by selecting **Resource groups** from the Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **SqlServer2008-ip** Public IP address from the list of resources.

    ![The SqlServer2008-ip IP address is highlighted in the list of resources.](media/sqlip-selection.png "SqlServer2008 public IP resource")

2. In the **Overview** blade, select **Copy** to copy the public IP address and paste the value into a text editor, such as Notepad.exe, for later reference.

    ![SqlServer2008-ip resource is open. Public IP Address copy button is highlighted.](media/sqlip-copy-public-ip.png "SqlServer2008 public IP")

3. Go back to the resource list and navigate to your **SQL database** resource by selecting the **parts** SQL database resource from the resources list.

    > **Note**: If you do not see the database resource, navigate to the SQL Server resource, then select **SQL Databases** and then the **parts** database.

   ![The parts SQL database resource is highlighted in the list of resources.](media/resources-azure-sql-database.png "SQL database")

4. On the Overview blade of your SQL database, copy the **Server name** and paste the value into a text editor, such as Notepad.exe, for later reference.

   ![The server name value is highlighted on the SQL database Overview blade.](media/sql-database-server-name.png "SQL database")

### Task 3: Migrate the database schema using the Data Migration Assistant

After reviewing the assessment results and ensuring the database is a candidate for migration to Azure SQL Database, use the Data Migration Assistant to migrate the schema to Azure SQL Database.

1. On the SqlServer2008 VM, return to the Data Migration Assistant and select the New **(+)** icon in the left-hand menu.

2. In the New project dialog, enter the following:

   - **Project type (1)**: Select Migration.
   - **Project name (2)**: Enter Migration.
   - **Source server type**: Select SQL Server.
   - **Target server type**: Select Azure SQL Database.
   - **Migration scope (3)**: Select Schema only.

   ![The above information is entered in the New Project dialog box.](media/data-migration-assistant-new-project-migration.png "New Project dialog")

3. Select **Create (4)**.

4. On the **Select source** tab, enter the following:

   - **Server name (1)**: Enter **SQLSERVER2008**.
   - **Authentication type (2)**: Select **SQL Server Authentication**.
   - **Username (3)**: Enter **PUWebSite**
   - **Password (4)**: Enter **{YOUR-ADMIN-PASSWORD}** `Password.1!!`
   - **Encrypt connection**: Check this box.
   - **Trust server certificate (5)**: Check this box.
   - Select **Connect (6)**, and then ensure the `PartsUnlimited` database is selected **(7)** from the list of databases.

   ![The Select source tab of the Data Migration Assistant is displayed, with the values specified above entered into the appropriate fields.](media/data-migration-assistant-migration-select-source.png "Data Migration Assistant Select source")

5. Select **Next (7)**.

6. On the **Select target** tab, enter the following:

   - **Server name (1)**: Paste the server name of your Azure SQL Database you copied into a text editor in the previous task.
   - **Authentication type (2)**: Select SQL Server Authentication.
   - **Username (3)**: Enter **demouser**
   - **Password (4)**: Enter **{YOUR-ADMIN-PASSWORD}** `Password.1!!`
   - **Encrypt connection**: Check this box.
   - **Trust server certificate (5)**: Check this box.
   - Select **Connect (6)**, and then ensure the `parts` database is selected **(7)** from the list of databases.

   ![The Select target tab of the Data Migration Assistant is displayed, with the values specified above entered into the appropriate fields.](media/data-migration-assistant-migration-select-target.png "Data Migration Assistant Select target")

7. Select **Next (8)**.

8. On the **Select objects** tab, leave all the objects checked **(1)**, and select **Generate SQL script (2)**.

    ![The Select objects tab of the Data Migration Assistant is displayed, with all the objects checked.](media/data-migration-assistant-migration-select-objects.png "Data Migration Assistant Select target")

9. On the **Script & deploy schema** tab, review the script. Notice the view also provides a note that there are no blocking issues **(1)**.

    ![The Script & deploy schema tab of the Data Migration Assistant is displayed, with the generated script shown.](media/data-migration-assistant-migration-script-and-deploy-schema.png "Data Migration Assistant Script & deploy schema")

10. Select **Deploy schema (2)**.

11. After the schema is deployed, review the deployment results, and ensure there were no errors.

    ![The schema deployment results are displayed, with 23 commands executed and 0 errors highlighted.](media/data-migration-assistant-migration-deployment-results.png "Schema deployment results")

12. Launch SQL Server Management Studio (SSMS) on the SqlServer2008 VM from the Windows Start menu by typing "sql server management" **(1)** into the search bar and then selecting **SQL Server Management Studio 17 (2)** in the search results.

    ![In the Windows Start menu, "sql server management" is entered into the search bar, and SQL Server Management Studio 17 is highlighted in the Windows start menu search results.](media/smss-windows-search.png "SQL Server Management Studio 17")

13. Connect to your Azure SQL Database by selecting **Connect->Database Engine** in the Object Explorer and then entering the following into the Connect to server dialog:

    - **Server name (1)**: Paste the server name of your Azure SQL Database you copied above.
    - **Authentication type (2)**: Select SQL Server Authentication.
    - **Username (3)**: Enter **demouser**
    - **Password (4)**: Enter **{YOUR-ADMIN-PASSWORD}** `Password.1!!`
    - **Remember password (5)**: Check this box.

    ![The SSMS Connect to Server dialog is displayed, with the Azure SQL Database name specified, SQL Server Authentication selected, and the demouser credentials entered.](media/ssms-connect-azure-sql-database.png "Connect to Server")

14. Select **Connect (6)**.

15. Once connected, expand **Databases**, and expand **parts**, then expand **Tables**, and observe the schema has been created **(1)**. Expand **Security > Users** to observe that the database user is migrated as well **(2)**.

    ![In the SSMS Object Explorer, Databases, parts, and Tables are expanded, showing the tables created by the deploy schema script. Security, Users are expended to show database user PUWebSite is migrated as well.](media/ssms-databases-contosoinsurance-tables.png "SSMS Object Explorer")

### Task 4: Migrate the database using the Azure Database Migration Service

At this point, you have migrated the database schema using DMA. In this task, you migrate the data from the `PartsUnlimited` database into the latest Azure SQL Database using the Azure Database Migration Service.

> The [Azure Database Migration Service](https://docs.microsoft.com/azure/dms/dms-overview) integrates some of the functionality of Microsoft's existing tools and services to provide customers with a comprehensive, highly available database migration solution. The service uses the Data Migration Assistant to generate assessment reports that provide recommendations to guide you through the changes required before performing a migration. When you're ready to begin the migration process, Azure Database Migration Service conducts all necessary steps.

1. In the [Azure portal](https://portal.azure.com), navigate to your Azure Database Migration Service by selecting **Resource groups** from the Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and then selecting the **contoso-dms-UniqueId** Azure Database Migration Service in the list of resources.

   ![The contoso-dms Azure Database Migration Service is highlighted in the list of resources in the hands-on-lab-SUFFIX resource group.](media/resource-group-dms-resource.png "Resources")

2. On the Azure Database Migration Service blade, select **+New Migration Project**.

   ![On the Azure Database Migration Service blade, +New Migration Project is highlighted in the toolbar.](media/dms-add-new-migration-project.png "Azure Database Migration Service New Project")

3. On the New migration project blade, enter the following:

   - **Project name (1)**: Enter DataMigration.
   - **Source server type**: Select SQL Server.
   - **Target server type**: Select Azure SQL Database.
   - **Choose type of activity**: Select **Offline data migration** and select **Save**.

   ![The New migration project blade is displayed, with the values specified above entered into the appropriate fields.](media/dms-new-migration-project-blade.png "New migration project")

4. Select **Create and run activity (2)**.

5. On the Migration Wizard **Select source** blade, enter the following:

   - **Source SQL Server instance name (1)**: Enter the IP address of your SqlServer2008 VM that you copied into a text editor in the previous task. For example, `51.143.12.114`.
   - **Authentication type (2)**: Select SQL Authentication.
   - **Username (3)**: Enter **PUWebSite**
   - **Password (4)**: Enter **{YOUR-ADMIN-PASSWORD}** `Password.1!!`
   - **Connection properties (5)**: Check both Encrypt connection and Trust server certificate.

   ![The Migration Wizard Select source blade is displayed, with the values specified above entered into the appropriate fields.](media/dms-migration-wizard-select-source.png "Migration Wizard Select source")

6. Select **Next: Select databases >> (6)**.

7. PartsUnlimited database comes preselected. Select **Next: Select target >>** to continue.

    ![The Migration Wizard Select database blade is displayed. PartsUnlimited database is selected. Next: Select target >> button is highlighted.](media/dms-migration-wizard-select-database.png "Migration Wizard Select databases")

8. On the Migration Wizard **Select target** blade, enter the following:

   - **Target server name (1)**: Enter the `fullyQualifiedDomainName` value of your Azure SQL Database (e.g., parts-xwn4o7fy6bcbg.database.windows.net), which you copied in the previous task.
   - **Authentication type (2)**: Select SQL Authentication.
   - **Username (3)**: Enter **demouser**
   - **Password (4)**: Enter **{YOUR-ADMIN-PASSWORD}** `Password.1!!`
   - **Connection properties**: Check Encrypt connection.

   ![The Migration Wizard Select target blade is displayed, with the values specified above entered into the appropriate fields.](media/dms-migration-wizard-select-target.png "Migration Wizard Select target")

9. Select **Next: Map to target databases >> (5)**.

10. On the Migration Wizard **Map to target databases** blade, confirm that **PartsUnlimited (1)** is checked as the source database, and **parts (2)** is the target database on the same line, then select **Next: Configuration migration settings >> (3)**.

    ![The Migration Wizard Map to target database blade is displayed, with the ContosoInsurance line highlighted.](media/dms-migration-wizard-map-to-target-databases.png "Migration Wizard Map to target databases")

11. On the Migration Wizard **Configure migration settings** blade, expand the **PartsUnlimited (1)** database and verify all the tables are selected **(2)**.

    ![The Migration Wizard Configure migration settings blade is displayed, with the expand arrow for PartsUnlimited highlighted, and all the tables checked.](media/dms-migration-wizard-configure-migration-settings.png "Migration Wizard Configure migration settings")

12. Select **Next: Summary >> (3)**.

13. On the Migration Wizard **Summary** blade, enter the following:

    - **Activity name**: Enter PartsUnlimitedDataMigration.

    ![The Migration Wizard summary blade is displayed, with PartsUnlimitedDataMigration entered into the name field.](media/dms-migration-wizard-migration-summary.png "Migration Wizard Summary")

14. Select **Start migration**.

15. Monitor the migration on the status screen that appears. Select the refresh icon in the toolbar to retrieve the latest status.

    ![On the Migration job blade, the Refresh button is highlighted, and a status of Full backup uploading is displayed and highlighted.](media/dms-migration-wizard-status-running.png "Migration status")

    > The migration takes approximately 2 - 3 minutes to complete.

16. When the migration is complete, you should see the status as **Completed**.

    ![On the Migration job blade, the status of Completed is highlighted.](media/dms-migration-wizard-status-complete.png "Migration with Completed status")

17. When the migration is complete, select the **PartsUnlimited** migration item.

    ![The ContosoInsurance migration item is highlighted on the PartsUnlimitedDataMigration blade.](media/dms-migration-completion.png "PartsUnlimitedDataMigration details")

18. Review the database migration details.

    ![A detailed list of tables included in the migration is displayed.](media/dms-migration-details.png "Database migration details")

19. If you received a status of "Warning" for your migration, you can find more details by selecting **Download report** from the ContosoDataMigration screen.

    ![The Download report button is highlighted on the DMS Migration toolbar.](media/dms-toolbar-download-report.png "Download report")

    > **Note**: The **Download report** button will be disabled if the migration is completed without warnings or errors.

20. The reason for the warning can be found in the Validation Summary section. In the report below, you can see that a storage object schema difference triggered a warning. However, the report also reveals that everything was migrated successfully.

    ![The output of the database migration report is displayed.](media/dms-migration-wizard-report.png "Database migration report")

### Task 5: Configure the application connection to SQL Azure Database

Now that we have both our application and database migrated to Azure. It is time to configure our application to use the SQL Azure Database.

1. In the [Azure portal](https://portal.azure.com), navigate to your `parts` SQL Database resource by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the `parts` SQL Database from the list of resources.

   ![The parts SQL database resource is highlighted in the list of resources.](media/resources-azure-sql-database.png "SQL database")

2. Switch to the **Connection strings (1)** blade, and copy the connection string by selecting the copy button **(2)**.

   ![Connection string panel if SQL Database is open. Copy button for ADO.NET connection string is highlighted.](media/sql-connection-string-copy.png "Database connection string")

3. Paste the value into a text editor, such as Notepad.exe, to replace the Password placeholder. Replace the `{your_password}` section with your admin password. Copy the entire connection string with the replaced password for later use.

    ![Notepad is open. SQL Connection string is pasted in. {your_password} placeholder is highlighted.](media/sql-connection-string-password-replace.png "Database connection string")

4. Go back to the resource list, navigate to your `partsunlimited-web-{uniquesuffix}` **(2)** App Service resource. You can search for `partsunlimited-web` **(1)** to find your Web App and App Service Plan.

   ![The search box for resource is filled in with partsunlimited-web. The partsunlimited-web-20 Azure App Service is highlighted in the list of resources in the hands-on-lab-SUFFIX resource group.](media/resource-group-appservice-resource.png "Resources")

5. Switch to the **Configuration (1)** blade, and select **+New connection string (2)**.

    ![App service configuration panel is open. +New connection string button is highlighted.](media/app-service-settings.png "App Service Configuration")

6. On the **Add/Edit connection string** panel, enter the following:

   - **Name(1)**: Enter `DefaultConnectionString`.
   - **Value**: Enter SQL Connection String you copied in Step 3.
   - **Type (3)**: Select **SQLAzure**
   - **Deployment slot setting (4)**: Check this option to make sure connection strings stick to a deployment slot. This will be helpful when we add additional deployment slots during the next exercises.

    ![Add/Edit Connection string panel is open. The name field is set to DefaultConnectionString. The value field is set to the connection string copied in a previous step. Type is set to SQL Azure. The deployment slot setting checkbox is checked. OK button is highlighted. ](media/app-service-connection-string.png "Adding connection string")

7. Select **OK (5)**.

8. Select **Save** and **Continue** for the following confirmation dialog.

    ![App Service Configuration page is open. Save button is highlighted.](media/app-service-settings-save.png "App Service Configuration")

9. Switch to the **Overview (1)** blade, and select **URL (2)** to navigate to the Parts Unlimited website hosted in our Azure App Service using Azure SQL Database.

    ![Overview panel for the App Service is on screen. URL for the app service is highlighted.](media/app-service-navigate-to-app-url.png "App Service public URL")

## Exercise 4: Setup CI/CD pipeline with GitHub Actions for the web app

Duration: 55 minutes

In this exercise, you will move the codebase to a GitHub Repo, create a staging environment in Azure App Service using App Service Deployment Slots, and finally connect the pieces with a CI/CD Pipeline built on GitHub Actions.

### Task 1: Moving the codebase to a GitHub repo

1. Log in to [GitHub](https://github.com) with your account. Select the New button positioned on top of the repositories list. As an alternative, you can [navigate to the new repository site here](https://github.com/new).

    ![GitHub.com Landing page is shown. New button to create a new repository is highlighted.](media/github-new-repo.png "GitHub new repo")

2. Type in `partsunlimited` **(1)** as your repository name. Select **Private (2)** to prevent public access to the repository. Select **Create repository (3)** to continue.

    ![Repository name is set to partsunlimited. Private access is selected. Create repository button is highlighted.](media/github-partsunlimited-repo.png "Create a new repository")

3. Select the **clipboard copy command** to copy the Git endpoint for your repository and paste the value into a text editor, such as Notepad.exe, for later reference.

    ![GitHub repository page is shown. Endpoint copy to clipboard button is highlighted.](media/github-endpoint-copy.png "Repository endpoint")

    So far, we have used the WebVM virtual machine to simulate Parts Unlimited's On-Premises IIS server. Now that we are done with the migration of Parts Unlimited's website. We will use the VM to execute some development tasks.

4. Connect to your WebVM VM with RDP.

   ![The WebVM virtual machine is highlighted in the list of resources.](media/webvm-selection.png "WebVM Selection")

5. Right-click on the Windows Start Menu and select **Windows PowerShell (Admin)** to launch a terminal window.

    ![Start Menu context menu is open. Windows PowerShell (Admin) command is highlighted.](media/launch-powershell.png "Windows PowerShell")

6. The Parts Unlimited website's source code is already copied into the VM as part of the **Before the hands-on lab exercises**. Run the command below to navigate to the source code folder.

    ```powershell
    cd "C:\MCW\MCW-App-modernization-main\Hands-on lab\lab-files\src"
    ```

7. Run the following command to initialize a local Git repository.

    ```powershell
    git init
    ```

    ![PowerShell terminal is shown. Git init is highlighted and executed.](media/git-init.png "Git init")

8. Next, we will define the remote endpoint as an origin to our local repository. Replace `{YourEndpointURL}` with the endpoint URL you copied previously from GitHub. Run the final command in your PowerShell terminal.

    ```powershell
    git remote add origin {YourEndpointURL}
    ```

    ![PowerShell terminal is shown. Git remote add origin is highlighted and executed. ](media/git-remote-add.png "Git remote add")

9. Run the following commands to rename the current branch to **Main** and stage all the files for a git commit.

    ```powershell
    git branch -M main
    git add .
    ```

10. Before we commit our changes, we have to identify our git user name and e-mail. In the following command, replace `John Doe` with your name and `johndoe@example.com` with your e-mail address. Once ready, run the command in your PowerShell terminal.

    ```powershell
    git config --global user.name "John Doe"
    git config --global user.email johndoe@example.com
    ```

11. We are ready to commit the source code to our local Git repository. Run the following command to continue.

    ```powershell
    git commit -m "Initial Commit"
    ```

12. Let's push our code to GitHub. Run the following command in your PowerShell terminal.

    ```powershell
    git push -u origin main
    ```

13. GitHub authentication screen will pop up. Select **Sign in with your browser (2)**. A new browser pop-up will appear with the GitHub login page.

    ![PowerShell terminal shows git push command and the GitHub Sign In experoence. Sign in with your browser button is highlighted.](media/github-sign-in.png "GitHub Sign In")

14. Fill-in your GitHub account credentials on the browser window to Sign-In.

15. On the **Authorize Git Credential Manager** screen, select **Authorize GotCredentialManager**. This will give your local environment permission to push the code to GitHub.

    ![Authorize Git Credential Manager is open. Authorize GotCredentialManager buttin is highligted.](media/github-access.png "Authorize Git Credential Manager")

16. Close the browser.

17. Go back to GitHub and observe the repository with the source code uploaded.

    ![GitHub is shown with the partsunlimited repository populated with source code.](media/github-partsunlimited-repo-loaded.png "GitHub repository page")

### Task 2: Creating a staging deployment slot

1. Go back to your lab resource group, navigate to your `partsunlimited-web-{uniquesuffix}` **(2)** App Service resource. You can search for `partsunlimited-web` **(1)** to find your Web App and App Service Plan

   ![The search box for resources is filled in with partsunlimited-web. The partsunlimited-web-20 Azure App Service is highlighted in the list of resources in the hands-on-lab-SUFFIX resource group.](media/resource-group-appservice-resource.png "Resources")

2. Switch to the **Deployment slots (1)** tab and select **Add Slot (2)**.

    ![App Service Deployment Slots tab is open. Add slot button highlighted.](media/app-service-add-deployment-slot.png "Deployment slots")

3. Type in **staging** as the name **(1)** of the new slot. Select your app service name from the **Clone settings from (2)** dropdown list. This will ensure our website configurations for the production environment are copied over to the staging environment as a starting point. Select **Add (3)** to add the new slot.

    ![Add a slot panel is open. The name is set staging. Partsunlimited-web-20 is selected for the clone settings from the dropdown list. Add button is highlighted.](media/app-service-staging-slot.png "Adding deployment slot")

4. Once you receive the success message, close **(1)** the panel. Observe **(2)** the two environments we have for the App Service in the deployment slots list.

    ![Successfully created slot staging message is shown. The close button is highlighted. The current list of slots is presented.](media/app-service-staging-slot-added.png "Deployment slots")

### Task 3: Setting up CI/CD with GitHub Actions

1. Select your staging slot from the list of deployment slots.

    ![Deployment slots are listed. Staging slot named partsunlimited-web-20-staging is highlighted.](media/app-service-staging-select.png "Staging deployment slot")

2. Switch to the **Deployment Center (1)** tab. Select **Go to Settings (2)**.

    ![Deployment Center tab is selected. Go to Settings button is highlighted.](media/app-service-goto-deployment-settings.png "Deployment Center")

3. Select **GitHub (1)** as your source; **.NET Core (2)** as the runtime stack and **.NET Core 2.1 (LTS) (3)** for version. Select **Authorize** to create the connection between the App Service deployment slot and the GitHub repository we previously prepared.

    ![Deployment Settings page is open. The source is set to GitHub. Runtime stack is set to .NET Core. Version is set to .NET Core 2.1 (LTS). Authorize button for GitHub is highlighted. ](media/app-service-deployment-settings.png "Deployment Center Settings")

4. Login with your GitHub credentials and provide authorization to App Service to access the repository by selecting **Authorize AzureAppService**.

    ![Authorize AzureAppService button is highlighted.](media/app-service-github-repo-access.png "Authorize Azure App Service")

5. Once GitHub authorization is complete, go back to the browser with the Azure Portal. Select the GitHub **Organization (1)** where you created the GitHub repository. This might be your personal account name if that is where you created the repository. Select the repository **partsunlimited (2)** and the branch **main (3)** as the source for the CI/CD pipeline. Select **Save (4)** to create CI/CD pipeline.

    ![Authorize AzureAppService button is highlighted.](media/app-service-cicd-settings-save.png "Deployment Center Settings")

    Once you select **Save**, the portal will add your App Service publishing profile as a secret to your GitHub repository. This will allow GitHub Actions to publish the Parts Unlimited website to the staging deployment slot. Additionally, the portal will create a YAML file that describes the steps required to build and publish the code in the partsunlimited repository.

6. Visit your GitHub repository on GitHub.com to look for changes. Navigate to `.github/workflows` **(1)** to see the **YAML file (2)** and the commit **(3)** made to the repository on your behalf.

    ![Partsunlimited repository is open on GitHub.com. .github/workflows folder is shown. A new commit that includes a main_partsunlimited-web-20(staging).yml file is highlighted.](media/github-inital-yaml-commit.png "GitHub Actions worklfow")

7. Select **Actions (1)** to navigate to the Actions page where you can see the list of workflow runs on the repository. Noticed that the latest run has failed **(2)**. Select the failed run (2) to investigate the issue.

    ![GitHub Actions for the repository is open](media/github-actions-failed.png "Github Actions")

8. Select the failed job to dig deeper.

    ![Details for the GitHub workflow run are shown. A failed job named build-and-deploy is highlighted.](media/github-actions-failed-net-core-version.png "GitHub Actions failed build")

9. In the error message, we can see a mismatch between the .NET Core version the build job is using and the one the project is built against. When we set up our CI/CD pipeline, the Azure Portal listed .NET Core LTS (Long Term Support) versions only. Unfortunately, Parts Unlimited uses a .NET Core version that hit the end of life on December 23, 2019. We will have to change our pipeline setup manually to accommodate project requirements.

    ![Build-and-deploy job error message is shown. SDK version requirement 2.2.207 is highlighted.](media/github-actions-version-error.png "Build Error")

10. Select **Code (1)** to switch back to the repository code view. Select **.github/workflows (2)** to navigate to the location where the workflow YAML code is stored.

    ![Partsunlimited GitHub repository root folder is shown. .github/workflows folders are highlighted. ](media/github-navigate-to-yaml.png "GitHub Actions Workflow")

11. Select the YAML file name `main_partsunlimited-web-20(staging).yml`.

    ![The main_partsunlimited-web-20(staging).yml file is highlighted in the GitHub / workflows folder.](media/github-select-yaml-file.png "Workflow YAML")

12. Select the **Edit this file (1)** button to modify the YAML file.

    ![The main_partsunlimited-web-20(staging).yml file is on screen. Edit this file button is highlighted.](media/github-yaml-edit.png "Workflow YAML Editing")

13. We have to change the **dotnet-version (1)** number to `2.2.207`. Additionally, we have to add the solution file name **(2)** and the project file name **(3)** to dotnet build and publish commands. The reason behind this change is the fact that Parts Unlimited has multiple solutions and projects in its codebase.

    ![main_partsunlimited-web-20(staging).yml is open in edit mode. dotnet-version is set to 2.2.207. dotnet build command is changed to include PartsUnlimited.sln as a parameter. dotnet publish command is changed to include src/PartsUnlimitedWebsite/PartsUnlimitedWebsite.csproj as a parameter.](media/github-yaml-commit.png "GitHub YAML Editing")

    Here is the final YAML file that you can use if needed.

    ```yaml
    
    # Docs for the Azure Web Apps Deploy action: https://github.com/Azure/webapps-deploy
    # More GitHub Actions for Azure: https://github.com/Azure/actions
    
    name: Build and deploy ASP.Net Core app to Azure Web App - partsunlimited-web-20(staging)
    
    on:
    push:
        branches:
        - main
    workflow_dispatch:
    
    jobs:
    build-and-deploy:
        runs-on: windows-latest
    
        steps:
        - uses: actions/checkout@main
    
        - name: Set up .NET Core
        uses: actions/setup-dotnet@v1
        with:
            dotnet-version: '2.2.207'
    
        - name: Build with dotnet
        run: dotnet build PartsUnlimited.sln --configuration Release
    
        - name: dotnet publish
        run: dotnet publish src/PartsUnlimitedWebsite/PartsUnlimitedWebsite.csproj -c Release -o ${{env.DOTNET_ROOT}}/myapp
    
        - name: Deploy to Azure Web App
        uses: azure/webapps-deploy@v2
        with:
            app-name: 'partsunlimited-web-20'
            slot-name: 'staging'
            publish-profile: ${{ secrets.AzureAppService_PublishProfile_a00d49c7adc84a028ccc74ff431024d5 }}
            package: ${{env.DOTNET_ROOT}}/myapp
    ```

14. Once all changes are complete, select **Start commit (4)**. Type a commit message **(5)**. Select **Commit changes (6)** to submit your changes to the repository.

15. Select **Actions (1)** to switch to the workflows page. Notice the latest successful run **(2)** of our workflow.

    ![Actions on the GitHub Repository is selected. The latest successful run of the workflow is highlighted.](media/github-actions-success.png "GitHub Actions success")

16. Go back to your lab resource group on the Azure Portal, navigate to your `staging (partsunlimited-web-{uniquesuffix}/staging)` **(2)** App Service resource. You can search for `staging` **(1)** to find your App Service (Slot) for staging.

    ![The search box for resources is filled in with staging. The staging (partsunlimited-web-{uniquesuffix}/staging) Azure App Service Deployment Slot is highlighted in the list of resources in the hands-on-lab-SUFFIX resource group.](media/select-staging-app-service.png "Staging Resource")

17. Notice the dedicated web link for your staging slot. Select to navigate to the website to see the result of your successful deployment through the CI/CD pipeline.

    ![Staging slot for partsunlimited app service is open. URL endpoint for the deployment slot is highlighted.](media/staging-slot-link.png "Staging public endpoint")

### Task 4: Pushing code changes to staging and production

1. Connect to your WebVM VM with RDP.

   ![The WebVM virtual machine is highlighted in the list of resources.](media/webvm-selection.png "WebVM Resource Selection")

2. Select the Start menu and search for **Visual Studio Code**. Select **Visual Studio Code** to run it.

    ![Start Menu is open. Visual Studio Code is typed in the search box. Visual Studio Code is highlighted from the list of search results.](media/vscode-start-menu.png "Visual Studio Code")

3. Open the **File (1)** menu and select **Open Folder... (2)**.

    ![File menu is open in Visual Studio Code. Open Folder... command is highlighted.](media/vscode-open-folder.png "Open Folder")

4. Navigate to `C:\MCW\MCW-App-modernization-main\Hands-on lab\lab-files\src` and select **Select Folder (1)**.

    ![Visual Studio Code Open Folder dialog is open. The folder path is set to C:\MCW\MCW-App-modernization-main\Hands-on lab\lab-files\src, and the Select Folder button is highlighted.](media/vscode-select-folder.png "Select Folder")

5. We are going to introduce a brand new change to Parts Unlimited's website. In the Explorer window navigate to **src > PartsUnlimitedWebSite > Views > Home** and select **Index.cshtml (4)** for editing. Change the Title of the page **(5)** and save the file by using going to the File menu and selecting **Save**. Notice the underlying git repository detecting a change (6) in the codebase.

    ![Index.cshtml from src > PartsUnlimitedWebSite > Views > Home folder is open. Page Title is changed to New Home Page. One pending change in the source control is highlighted.](media/vscode-changing-source-code.png "Code editing in VSCode")

6. Select **Source Control (1)** tab in Visual Studio Code. Since we worked on the codebase in our repo in the virtual machine, the codebase in the repo on GitHub has changed. Open the **Views and more actions... (2)** menu and select **Pull (3)** to get the latest from the remote repository.

    ![Views and more actions... menu is open. Pull command is highlighted.](media/vscode-pull.png "GitHub Pull")

7. Select **Stage Changes (1)**. Type in a commit message **(2)** for the changes. Select **Commit (3)**.

    ![Stage changes button for index.cshtml is highlighted. Commit message is set to New Home Page Title. Commut button is highlighted.](media/vscode-stage-commit.png "GitHub Commit")

8. Open the **Views and more actions... (1)** menu and select **Push (2)** to push the changes to GitHub.

    ![Views and more actions... menu is open. Push command is highlighted.](media/vscode-push.png "GitHub Push")

9. Open the GitHub repository and observe the Actions page for the latest execution of the CI/CD Pipeline.

    ![PartsUnlimited repo is open. Actions page is shown. Successful CI/CD run for the new home page title is highlighted.](media/github-actions-success-commit.png "New commit build")

10. Navigate to the staging environment endpoint in your browser and observe the Title change.

    ![Parts Unlimited staging environment is open in a browser. New Home Page title is highlighted.](media/staging-code-changes.png "Parts Unlimited Staging Web Site")

Now that Parts Unlimited has a separate staging environment for their e-commerce site, they can push new source code and functionality to the repo that will automatically be built and deployed to their staging for testing.

### Task 5: Swap deployment slots to move changes in staging to production

Once Parts Unlimited is happy with the changes tested in their staging environment, they can swap the two environments and have changes go to production. Environment Swap happens very fast and can help Parts Unlimited pull back changes by switching back if needed.

1. Go back to your lab resource group, navigate to your `partsunlimited-web-{uniquesuffix}` **(2)** App Service resource. You can search for `partsunlimited-web` **(1)** to find your app service.

   ![The search box for resources is filled in with partsunlimited-web. The partsunlimited-web-20 Azure App Service is highlighted in the list of resources in the hands-on-lab-SUFFIX resource group.](media/resource-group-appservice-resource.png "Resources")

2. Switch to the **Deployment slots (1)** tab and select **Swap (2)**.

    ![App Service Deployment Slots tab is open. Swap button highlighted.](media/app-service-slot-swap.png "Deployment Slot Swap")

3. Select the **Swap** button to swap the staging slot with the production slow.

    ![Deployment Slot Swap dialog is open. Swap button is highlighted.](media/app-service-slot-swap-panel.png "Deployment Slot Swap")

4. Once you receive the success message, close the swap panel.

5. Visit both production and staging slot endpoints and observe how the Title change is moved to production.

    > Once you move your staging slot to production, your production slot is moved to staging as well. This means that your current staging slot does not have the latest changes you have pushed to the repo. You can trigger a manual CI/CD workflow execution to push the latest changes to staging.
    >
    > To run a CI/CD workflow manually, go to GitHub actions page **(1)** in your repository. Select the workflow **(2)** to run. Open the **Run workflow (3)** menu and select **Run workflow (4)**.
    >
    > ![GitHub Actions page is shown. Build and deploy ASP.Net Core app to Azure Web App - partsunlimited-web-20(staging) workflow is selected. Run workflow menu is open. Run workflow button is highlighted.](media/github-actions-manual-run.png "Manual workflow run")

## Exercise 5: Using serverless Azure Functions to process orders

Duration: 30 minutes

With its migration to Azure, Parts Unlimited plans to launch a series of campaigns to increase its sales. Their current architecture is processing purchase orders synchronously and is coupled with their front end. Parts Unlimited is looking for ways to decouple its order processing system and make sure it can scale independently from the web front end when orders increase.

You suggest a serverless approach that can handle order processing and the creation of invoices for processed orders. In this exercise, you will deploy a serverless Azure Function that will listen to an Azure Storage Queue and process orders as they come. Parts Unlimited has already implemented the changes required to push the jobs into a queue of your choice.

### Task 1: Deploying Azure Functions

1. Connect to your WebVM VM with RDP.

   ![The WebVM virtual machine is highlighted in the list of resources.](media/webvm-selection.png "WebVM Resource Selection")

2. Select the Start menu and search for **Visual Studio Code**. Select **Visual Studio Code** to run it.

    ![Start Menu is open. Visual Studio Code is typed in the search box. Visual Studio Code is highlighted from the list of search results.](media/vscode-start-menu.png "Visual Studio Code")

3. Open the **File (1)** menu and select **Open Folder... (2)**.

    ![File menu is open in Visual Studio Code. Open Folder... command is highlighted.](media/vscode-open-folder.png "Open Folder")

4. Navigate to `C:\MCW\MCW-App-modernization-main\Hands-on lab\lab-files\src-invoicing-functions\FunctionApp` and select **Select Folder**.

5. Select **Install** to install extensions required for your Azure Functions project. This will install C# for Visual Studio Code and Azure Functions Extension for Visual Studio Code.

    ![Visual Studio Code is on screen. Install extensions button is highlighted.](media/vscode-extenstions-install.png "Extension Install")

6. Once install is **Finished (1)** select **Restore (2)** to download dependencies for the project.

    ![Visual Studio Code is on screen. Restore dependencies button is highlighted.](media/vscode-restore-dependencies.png ".NET Restore")

7. When restore is complete close the tabs titled **Extension (1) (2)** and the **welcome tab (3)**. Select **Azure (4)** from the left menu and select **Sign into Azure (5)**. Select **Edge** as your browser if requested.

    ![Clouse buttons for all tabs are highlighted. Azure button from the left bar is selected. Sign in to Azure link is highlighted.](media/vscode-azure-signin.png "VSCode Azure Sign In")

8. Enter your Azure credentials and Sign In.

9. Close the browser window once your sign-in is complete.

10. Drill down **(1)** the resource in your subscription. Right-click on your Azure Function named **parts-func-{uniquesuffix} (2)** and select **Deploy to Azure Function App... (3)**.

    ![Azure subscription is shown. The function app **parts-func-{uniquesuffix} (2)** is selected. On the context menu, Deploy to Function App is highlighted.](media/vscode-deploy-function-app.png "Deploy to Function App")

11. Select **Deploy**.

    ![VS Code deployment approval dialog is open.](media/vscode-deploy-approval.png "Deploy overwrite")

### Task 2: Connecting Function App and App Service

1. In the [Azure portal](https://portal.azure.com), navigate to your `parts` Storage Account resource by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the `parts{uniquesuffix}` Storage Account from the list of resources.

    ![The parts{uniquesuffix} Storage Account is highlighted in the list of resources in the hands-on-lab-SUFFIX resource group.](media/select-storage-account.png "Storage Resource Selection")

2. Switch to the **Access keys (1)** blade, and select **Show keys (2)**. Select the copy button for the first connection string **(3)**. Paste the value into a text editor, such as Notepad.exe, for later reference.

    ![Access keys blade is open. Show keys button is highlighted. The copy button for the first connection string is pointed.](media/storage-account-connection-copy.png "Storage Access Keys")

3. Go back to the resource list and navigate to your `partsunlimited-web-{uniquesuffix}` **(2)** App Service resource. You can search for `partsunlimited-web` **(1)** to find your Web App and App Service Plan

   ![The search box for the resource is filled in with partsunlimited-web. The partsunlimited-web-20 Azure App Service is highlighted in the list of resources in the hands-on-lab-SUFFIX resource group.](media/resource-group-appservice-resource.png "Resources")

4. Switch to the **Configuration (1)** blade, and select **+New connection string (2)**.

    ![App service configuration panel is open. +New connection string button is highlighted.](media/app-service-settings-new-connection.png "App Service Configuration")

5. On the **Add/Edit connection string** panel, enter the following:

   - **Name(1)**: Enter `StorageConnectionString`.
   - **Value**: Enter Storage Connection String you copied in Step 2.
   - **Type (3)**: Select **Custom**
   - **Deployment slot setting (4)**: Check this option to make sure connection strings stick to a deployment slot. This will make sure you can have different settings for staging and production.

    ![Add/Edit Connection string panel is open. The name field is set to StorageConnectionString. The value field is set to the connection string copied in a previous step. Type is set to Custom. The deployment slot setting checkbox is checked. OK button is highlighted. ](media/app-service-storage-connection.png "Deployment Slot Configuration")

6. Select **OK (5)**.

7. Select **Save** and **Continue** for the following confirmation dialog.

    ![App Service Configuration page is open. Save button is highlighted.](media/app-service-settings-save.png "App Service Configuration")

8. Go back to the resource list and navigate to your `parts-func-{uniquesuffix}` **(2)** Function App resource. You can search for `func` **(1)** to find your function app.

   ![The search box for the resource is filled in with func. The parts-func-{uniquesuffix} Function App is highlighted in the list of resources in the hands-on-lab-SUFFIX resource group.](media/select-function-app.png "Function App Resource Selection")

9. Switch to the **Configuration (1)** blade, and select **+New application setting (2)**.

    ![Function App configuration panel is open. +New application setting button is highlighted.](media/function-app-app-settings-new.png "Function App Configuration")

10. On the **Add/Edit connection string** panel, enter the following:

    - **Name(1)**: Enter `DefaultConnection`.
    - **Value**: Enter SQL Connection String you copied in Exercise 3, Task 5, Step 3.

    ![Add/Edit Connection string panel is open. The name field is set to StorageConnectionString. The value field is set to the connection string copied in a previous step. Type is set to Custom. The deployment slot setting checkbox is checked. OK button is highlighted.](media/function-app-sql-setting.png "Function App Configuration")

11. Select **OK (3)**.

12. Select **Save** and **Continue** for the following confirmation dialog.

    ![Function App Configuration page is open. Save button is highlighted.](media/function-app-setting-save.png "Function App Configuration")

### Task 3: Testing serverless order processing

In this task, we will submit a new order on the Parts Unlimited website and observe the order's processing on the order details page. Once the order is submitted, the web front-end will put a job into an Azure Storage Queue. The Function App that we previously deployed is set to listen to the queue and pull jobs for processing. Once order processing is done, a PDF file will be created, and the link for the PDF file will be accessible on the order details page.

1. Go back to the resource list and navigate to your `partsunlimited-web-{uniquesuffix}` **(2)** App Service resource. You can search for `partsunlimited-web` **(1)** to find your Web App and App Service Plan.

   ![The search box for the resource is filled in with partsunlimited-web. The partsunlimited-web-20 Azure App Service is highlighted in the list of resources in the hands-on-lab-SUFFIX resource group.](media/resource-group-appservice-resource.png "Resources")

2. Select **URL** and navigate to the Parts Unlimited website hosted in Azure App Service.

    ![Parts Unlimited App Service is on screen. URL is highlighted.](media/navigate-to-parts-unlimited-app-service.png "App Service Public Endpoint")

3. Select **Login (1)** and select **Register as a new user? (2)**.

    ![Parts Unlimited website login screen is presented. Log in and Register as a new user buttons are highlighted.](media/register-parts-unlimited.png "Parts Unlimited Login")

4. Type in `test@test.com` for the email **(1)** and `Password.1!!` **(2)** for the password. Select **Register (3)**.

    ![Parts Unlimited website user registration screen is presented. E-mail box is filled with test@test.com. Password boxes are filled in with Password.1!!. The register button is highlighted.](media/register-parts-unlimited-new-user.png "Register")

5. On the next screen, select **Click here to confirm your e-mail** to confirm your e-mail.

6. Select **Login (1)** and type the credentials listed below.

    - **Email (2):** test@test.com
    - **Password (3):** Password.1!!

    ![Parts Unlimited website login screen is presented. The E-mail box is filled with test@test.com. Password boxes are filled in with Password.1!!. The login button is highlighted. ](media/parts-umlimited-login.png "Login")

7. Select **Login (4)**.

8. Select a product from the home page, and select **Add to Card** once you are on the product detail page.

    ![Product detail page is shown. Add to cart button is highlighted.](media/parts-unlimited-add-to-cart.png "Add to Cart")

9. Select **Checkout** on the next screen.

10. Fill in sample shipping information **(1)** for testing purposes. Use coupon code **FREE (2)** and select **Submit Order (3)**.

    ![Sample shipping information is filled in. FREE coupon code is typed in. Submit Order button is highlighted.](media/parts-unlimited-order.png "Submit Order")

11. Once checkout is complete select **view your order** to see order details.

    ![Checkout Complete page is shown. View your order link is highlighted.](media/parts-unlimited-view-order-details.png "View your order")

12. Observe the invoice field. Right now, your order is flagged as in processing. An order job is submitted to the Azure Storage Queue to be processed by Azure Functions. Refresh the page every 15 seconds to see if anything changes about your order.

    ![Order details page is open. The invoice field is highlighted.](media/parts-unlimited-invoice-processing.png "Invoice processing")

13. Once your order has been processed, an invoice will be created, and a download link will appear. Select the download link to download the PDF invoice created for your order by Azure Functions.

    ![Order details page is open. Invoice field is highlighted to show a download link.](media/parts-unlimited-pdf-download.png "Invoice download")

    Here is your invoice.

    ![A sample Parts Unlimited Invoice is presented.](media/invoice-pdf.png "Sample Invoice")

### Task 4: Enable Application Insights on the Function App

In this task, you add Application Insights to your Function App in the Azure Portal to be able to collect insights related to Function executions.

1. In the [Azure portal](https://portal.azure.com), navigate to your **Function App** by selecting **Resource groups** from Azure services list, selecting the **hands-on-lab-SUFFIX** resource group, and selecting the **parts-func-{uniquesuffix}** App service from the list of resources.

   ![The Function App resource is highlighted in the list of resources.](media/azure-resources-function-app.png "Function App")

2. On the Function App blade, select **Application Insights (1)** under Settings from the left-hand menu. On the Application Insights blade, select **Turn on Application Insights (2)**.

   ![Application Insights blade is selected. The Turn on Application Insights button is highlighted.](media/function-app-add-app-insights.png "Turn on Application Insights for Function App")

3. On the Application Insights blade, select **Create new resource (1)**, accept the default name provided, and then select **Apply (2)**. Select **Yes (3)** when prompted about restarting the Function App to apply monitoring settings.

   ![The Create New Application Insights blade is displayed with a unique name set under Create new resource. Apply and the following Yes approval buttons are highlighted.](media/function-app-app-insights.png "Add Application Insights")

4. After the Function App restarts, select **View Application Insights data**.

   ![The View Application Insights data link is highlighted.](media/function-app-view-application-insights-data.png "View Application Insights data")

5. On the Application Insights blade, select **Live Metrics Stream (1)** from the left-hand menu.

   ![Live Metrics Stream is highlighted in the left-hand menu on the Application Insights blade.](media/app-insights-live-metrics-stream.png "Application Insights")

   > While having Live Metric up, try submitting a new order on the Parts Unlimited website. You will see access to blob storage in the telemetry to upload the PDF **(2)** and execution count on the graph **(3)**.

## After the hands-on lab

Duration: 15 minutes

In this exercise, you will de-provision all Azure resources created in support of this hands-on lab.

### Task 1: Delete Azure resource groups

1. In the Azure portal, select **Resource groups** from the Azure services list and locate and delete the **hands-on-lab-SUFFIX** resource group.

### Task 2: Delete GitHub repository

1. From your GitHub account, find your `partsunlimited` repository by searching for `parts` **(1)** and selecting the repository **(2)**.

    ![GitHub main page is on display. The repository search box is filled with parts. Partsunlimited repository is highlighted.](media/github-find-repo.png "Parts GitHub repository")

2. When you are on the repository page, select **Settings**.

    ![Repository page is open. The settings tab is highlighted.](media/github-repo-settings.png "GitHub Repository Settings")

3. Scroll down on the Settings page and select the **Delete this repository** button in the Danger Zone.

    ![Repository settings page is open. Delete this repository button is highlighted.](media/github-repo-delete.png "Delete this repository")

4. Type in the entire repository name **(2)** as displayed **(1)** and confirm the deletion by selection **I understand the consequences, delete this repository** button.

    ![Repository name is highlighted and typed in into the confirmation textbox. "I understand the consequences, delete this repository" button is highlighted.](media/github-repo-delete-approval.png "Deletion Approval")

    > You might be asked for your GitHub password for the second round of approval.

### Task 3: Remove GitHub Authorized Apps

1. Log in to GitHub with your account.

2. Navigate to [https://github.com/settings/applications](https://github.com/settings/applications) in the same browser window/tab.

3. From the list of authorized app **Revoke (1)** access to applications listed below.

    > **WARNING:** Revoking the permissions listed above will disconnect these applications from your GitHub Account. If you have been using these applications before this lab, you might want to keep the permissions. Otherwise, other environments that you control and have access to your GitHub account might lose access as well.

    - Azure App Service
    - GitHub for VSCode
    - Git Credential Manager

    ![GitHub Authorized Applications are listed. Azure App Service, GitHub for VSCode, and Git Credential Manager are highlighted. Revoke button for Azure App Service is shown.](media/github-authorized-apps.png "GitHub Applications")

You should follow all steps provided *after* attending the Hands-on lab.
