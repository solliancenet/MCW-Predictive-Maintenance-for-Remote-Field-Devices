![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png 'Microsoft Cloud Workshops')

<div class="MCWHeader1">
Predictive Maintenance for Remote Field Devices
</div>

<div class="MCWHeader2">
Whiteboard design session student guide
</div>

<div class="MCWHeader3">
July 2019
</div>

Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only, and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third-party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2019 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are the property of their respective owners.

**Contents**

<!-- TOC -->

- [Predictive Maintenance for Remote Field Devices whiteboard design session student guide](#Predictive-Maintenance-for-Remote-Field-Devices-whiteboard-design-session-student-guide)
  - [Abstract and learning objectives](#Abstract-and-learning-objectives)
  - [Step 1: Review the customer case study](#Step-1-Review-the-customer-case-study)
      - [Customer situation](#Customer-situation)
    - [Customer needs](#Customer-needs)
    - [Customer objections](#Customer-objections)
    - [Infographic for common scenarios](#Infographic-for-common-scenarios)
  - [Step 2: Design a proof of concept solution](#Step-2-Design-a-proof-of-concept-solution)
  - [Step 3: Present the solution](#Step-3-Present-the-solution)
  - [Wrap-up](#Wrap-up)
  - [Additional references](#Additional-references)

<!-- /TOC -->

# Predictive Maintenance for Remote Field Devices whiteboard design session student guide

## Abstract and learning objectives

This whiteboard design session is designed to help you gain a better understanding of implementing architectures that ...

At the end of this whiteboard design session, you will be better able to design ...

## Step 1: Review the customer case study

**Outcome**

Analyze your customer's needs.

Timeframe: 15 minutes

Directions: With all participants in the session, the facilitator/SME presents an overview of the customer case study along with technical tips.

1.  Meet your table participants and trainer.

2.  Read all of the directions for steps 1-3 in the student guide.

3.  As a table team, review the following customer case study.

#### Customer situation

Fabrikam, Inc. creates innovated IoT solutions for the oil and gas manufacturing industry. It is beginning work on a new, predictive maintenance solution that targets rod pumps (the iconic pivoting pumps that dot oil fields around the world). With their solution in place, companies will be able to monitor and configure pump settings and operations remotely, and only send personnel onsite when necessary for repair or maintenance when the solution indicates that something has gone wrong. However, Fabrikam wants to go beyond reactive alerting- they want to enable the solution with the ability to predict problems so they can be averted before a fault occurs and damage is done.

Using Machine Learning gives Fabrikam's solution the ability to analyze readings from various elements of the rod pump mechanism and sense patterns that indicate a possible impending mechanical failure or a deviation from the pump’s optimal operating conditions. In that case, the controller can modify the operating parameters of the pump to avoid or mitigate the impact of the unexpected changes. Or, if necessary, it can shut down the pump before any damage occurs and notify the company that repairs are necessary—protecting the machinery, and preventing potential environmental damage.

Their goal in the use of such predictive models is to increase operator efficiency and safety. Addressing a typical maintenance issue takes several people and at least three days of system downtime at a cost of up to $20,000 USD a day, not including parts and labor. "By proactively identifying pump problems through predictive analytics, companies reduce unplanned downtime, which decreases costs, increases production, and increases the agility of maintenance services." says Fabrikam's Chief Engineer. He adds that the majority of industry accidents don’t happen at the well site, they happen when personnel are driving between sites. By eliminating the need for many site visits, they can reduce those accidents.

They would like to understand their options for expediting the implementation of the PoC. Specifically they are looking to learn what offerings Azure provides that could enable a quick end-to-end start on the infrastructure for monitoring and managing devices and the system metadata. On top of this, they are curious about what other platform services Azure provides that they should consider in this scenario.

They would like to start by building a proof of concept that performs the predictive analytics in the cloud. While their machine learning will initially happen in the cloud, they would like to design their solution with an eye towards the future so it could be enhanced to run the models at the edge.

### Customer needs

1. They need web based access to metadata management
2. They need configurable dashboards
3. They need collection and export of real-time telemetry
4. They need security


### Customer objections

1. Is there an out of the box solution they can use to jumpstart the creation of the solution?
2. Can this solution enable their data scientists to leverage their expertise to build custom models against timeseries telemetry, and to plug their models into the near-real time streams collected by the solution?
3. How would data engineers get at rod pump streaming telemetry?
4. How can data engineers collect historical streaming telemetry for use in modeling by the data scientists?
5. What service would the data scientists use for training their model that could support the potentially large scale?
6. How could data scientists plug their model into a real-time predictive analytics pipeline to detect potential failures?
7. How would alerts be generated and delivered when the model predicts a failure?
8. How can the operators view the current status of the rod pumps in a single dashboard?
   
### Infographic for common scenarios

image goes here...

## Step 2: Design a proof of concept solution

**Outcome**

Design a solution and prepare to present the solution to the target customer audience in a 15-minute chalk-talk format.

Timeframe: 60 minutes

**Business needs**

Directions: With all participants at your table, answer the following questions and list the answers on a flip chart:

1.  Who should you present this solution to? Who is your target customer audience? Who are the decision makers?

2.  What customer business needs do you need to address with your solution?

**Design**

Directions: With all participants at your table, respond to the following questions on a flip chart:

_High-level architecture_

1.  Without getting into the details (the following sections will address the particular details), diagram your initial vision for ...

_IoT Central_

...

_Metadata Management_

...

_Dashboards_

...

_Security_

...

**Prepare**

Directions: With all participants at your table:

1.  Identify any customer needs that are not addressed with the proposed solution.

2.  Identify the benefits of your solution.

3.  Determine how you will respond to the customer's objections.

Prepare a 15-minute chalk-talk style presentation to the customer.

## Step 3: Present the solution

**Outcome**

Present a solution to the target customer audience in a 15-minute chalk-talk format.

Timeframe: 30 minutes

**Presentation**

Directions:

1.  Pair with another table.

2.  One table is the Microsoft team and the other table is the customer.

3.  The Microsoft team presents their proposed solution to the customer.

4.  The customer makes one of the objections from the list of objections.

5.  The Microsoft team responds to the objection.

6.  The customer team gives feedback to the Microsoft team.

7.  Tables switch roles and repeat Steps 2-6.

## Wrap-up

Timeframe: 15 minutes

Directions: Tables reconvene with the larger group to hear the facilitator/SME share the preferred solution for the case study.

## Additional references

...