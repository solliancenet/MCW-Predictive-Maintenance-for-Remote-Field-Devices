![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png 'Microsoft Cloud Workshops')

<div class="MCWHeader1">
Predictive Maintenance for remote field devices
</div>

<div class="MCWHeader2">
Before the hands-on lab setup guide
</div>

<div class="MCWHeader3">
September 2019
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2019 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents**

<!-- TOC -->

- [Predictive Maintenance for remote field devices before the hands-on lab setup guide](#predictive-maintenance-for-remote-field-devices-before-the-hands-on-lab-setup-guide)
  - [Requirements](#requirements)
  - [Before the hands-on lab](#before-the-hands-on-lab)
    - [Task 1: Provision a resource group](#task-1-provision-a-resource-group)
    - [Task 2: Create an Azure Databricks service](#task-2-create-an-azure-databricks-service)
    - [Task 3: Create Azure Databricks cluster](#task-3-create-azure-databricks-cluster)
    - [Task 4: Import lab notebooks into Azure Databricks](#task-4-import-lab-notebooks-into-azure-databricks)
    - [Task 5: Create Azure Machine Learning service workspace](#task-5-create-azure-machine-learning-service-workspace)

<!-- /TOC -->

# Predictive Maintenance for remote field devices before the hands-on lab setup guide

## Requirements

1. Microsoft Azure subscription (non-Microsoft subscription, must be a pay-as-you subscription).

## Before the hands-on lab

Duration: 60 minutes

In the Before the hands-on lab exercise, you will set up your environment for use in the rest of the hands-on lab. You should follow all the steps provided in the Before the hands-on lab section to prepare your environment **before attending** the hands-on lab. Failure to do so will significantly impact your ability to complete the lab within the time allowed.

> **Important**: Most Azure resources require unique names. Throughout this lab you will see the word “SUFFIX” as part of resource names. You should replace this with your Microsoft alias, initials, or another value to ensure the resource is uniquely named.

### Task 1: Provision a resource group

In this task, you will create an Azure resource group for the resources used throughout this lab.

1. Log into the [Azure Portal](https://portal.azure.com)

2. In the left-hand menu, select **Resource Groups**.

3. At the top of the screen press the **Add** button.

   ![Add Resource Group Menu](media/add-resource-group-menu.png)

4. Create a new resource group with the name **Fabrikam_Oil**, ensure the proper subscription and region nearest you are selected. Then select **Review + Create**.

   ![Create Resource Group](media/create-resource-group.png)

5. On the Summary blade, select **Create** to provision your resource group.

### Task 2: Create an Azure Databricks service

Azure Databricks is used to train and deploy a machine learning model that predicts an oil pump failure, based on incoming telemetry.

1. Navigate to the [Azure portal](https://portal.azure.com).

2. Select **+ Create a resource**, type in "Databricks" in the search field, then select **Azure Databricks** from the results.

   ![Create a resource is highlighted and Azure Databricks is selected.](media/azure-create-databricks-search.png 'SQL Database')

3. Select **Create** in the Azure Databricks details page.

4. Within the **Azure Databricks Service** form, complete the following:

   | Field                          | Value                                      |
   | ------------------------------ | ------------------------------------------ |
   | Workspace name                 | _globally unique name_                     |
   | Subscription                   | _select the appropriate subscription_      |
   | Resource Group                 | _select use existing, then `Fabrikam_Oil`_ |
   | Location                       | _select the location nearest to you_       |
   | Pricing tier                   | _select Standard_                          |
   | Deploy in your Virtual Network | _select No_                                |

   ![The form fields are completed with the previously described settings.](media/azure-create-databricks.png 'Azure Databricks Service')

5. Select **Create**.

### Task 3: Create Azure Databricks cluster

1. In the [Azure portal](https://portal.azure.com), open your Azure Databricks service you created in the previous task.

2. Select **Launch Workspace**. Azure Databricks will automatically sign you in through its Azure Active Directory integration.

   ![Launch Workspace](media/databricks-launch-workspace.png 'Launch Workspace')

3. Once in the workspace, select **Clusters** in the left-hand menu, then select **+ Create Cluster**.

   ![Create Cluster is highlighted.](media/databricks-clusters.png 'Clusters')

4. In the **New Cluster** form, specify the following configuration options:

   | Field                      | Value                                                                                        |
   | -------------------------- | -------------------------------------------------------------------------------------------- |
   | Cluster name               | _enter `lab`_                                                                                |
   | Cluster Mode               | _select `Standard`_                                                                          |
   | Pool                       | _select `None`_                                                                              |
   | Databricks Runtime Version | _select `Runtime 5.5 LTS (Scala 2.11, Spark 2.4.3)`_                                         |
   | Python Version             | _enter `3`_                                                                                  |
   | Autopilot Options          | _uncheck `Enable autoscaling` and check `Terminate after...`, with a value of `120` minutes_ |
   | Worker Type                | _select `Standard_DS3_v2`_                                                                   |
   | Driver Type                | _select `Same as worker`_                                                                    |
   | Workers                    | _enter `1`_                                                                                  |

   ![The New Cluster form is displayed with the previously described values.](media/databricks-new-cluster.png 'New Cluster')

5. Select **Create Cluster**.

### Task 4: Import lab notebooks into Azure Databricks

In this task, you will import the Databricks notebooks into your workspace.

1. Within your Azure Databricks service, select **Workspace**, select **Users**, select the dropdown to the right of your username, then select **Import**.

   ![The Import link is highlighted in the Workspace.](media/databricks-import-link.png 'Workspace')

2. Select **URL** next to **Import from**, paste the following into the text box: `https://github.com/microsoft/MCW-Predictive-Maintenance-for-Remote-Field-Devices/blob/master/Hands-on%20lab/Resources/Notebooks/Anomaly-Detection.dbc`, then select **Import**.

   ![The URL has been entered in the import form.](media/databricks-import.png 'Import Notebooks')

3. After importing, select your username. You will see a new folder named `Anomaly Detection`, which contains two notebooks.

   ![The imported notebooks are displayed.](media/databricks-notebooks.png 'Imported notebooks')

### Task 5: Create Azure Machine Learning service workspace

1. Navigate to the [Azure portal](https://portal.azure.com).

2. Select **+ Create a resource**, type in "machine learning" in the search field, then select **Machine Learning service workspace** from the results.

   ![Create a resource is highlighted and Machine Learning service workspace is selected.](media/azure-create-aml-search.png 'SQL Database')

3. Select **Create** in the Azure Databricks details page.

4. Within the **Azure Databricks Service** form, complete the following:

   | Field          | Value                                      |
   | -------------- | ------------------------------------------ |
   | Workspace name | _globally unique name_                     |
   | Subscription   | _select the appropriate subscription_      |
   | Resource Group | _select use existing, then `Fabrikam_Oil`_ |
   | Location       | _select the location nearest to you_       |

   ![The form fields are completed with the previously described settings.](media/azure-create-aml.png 'Machine Learning service workspace')

5. Select **Create**.

You should follow all steps provided _before_ performing the Hands-on lab.
