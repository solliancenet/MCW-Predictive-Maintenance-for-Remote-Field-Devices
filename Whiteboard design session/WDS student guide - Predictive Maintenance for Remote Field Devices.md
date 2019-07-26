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
      - [Telemetry data](#Telemetry-data)
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

Fabrikam, Inc. creates innovative IoT solutions for the oil and gas manufacturing industry. It is beginning work on a new, predictive maintenance solution that targets rod pumps (the iconic pivoting pumps that dot oil fields around the world). With their solution in place, companies will be able to monitor and configure pump settings and operations remotely, and only send personnel onsite when necessary for repair or maintenance when the solution indicates that something has gone wrong. However, Fabrikam wants to go beyond reactive alerting- they want to enable the solution with the ability to recognize problems, based on automated observation of telemetry data, so they can be averted before a fault occurs and the damage is done.

One of Fabrikam's challenges is managing IoT devices at scale. They want to be able to securely store credentials on their devices before sending them to their customers. When the devices connect to the cloud, they want to have an approval process that activates those devices so they can start sending telemetry securely. All data in transit must be encrypted.

Another decision point for Fabrikam is whether they would benefit from a software-as-a-service (SaaS) cloud-based offering, or a more customized platform-as-a-service (PaaS) solution. Fabrikam's technical leadership believes that, since IoT is not one of their top disciplines, and because technology is rapidly changing in the IoT space, they would benefit most from a SaaS-based solution. On the other hand, they want to know what the range of IoT PaaS options there are on Azure so they can make an informed decision on which route they should invest their time and resources. Given their current lack of IoT experience, Fabrikam would like to build a proof of concept using a SaaS product that they can quickly get started with to evaluate whether it meets their immediate and long-term needs. These needs include device management, visualization, reporting, control, and monitoring. Ideally, the solution will be highly scalable and secure, should they decide to transition from a PoC to a pilot deployment with several customers. When they do start expanding to several customers, they would like a transparent pricing structure they can use to project costs and pass those costs to their customers based on their usage.

Whichever cloud-based solution Fabrikam pursues, they want to know how they can visualize all of the data generated by sensors on each rod pump. These sensors include power output from motors, motor speed, the pump rate measured in strokes per minute, how long the pump is on, and casing friction measured in psi, to name a few. Each of these sensors output data at different rates and measurements, complicating any effort to build meaningful charts and dashboards. The information needs to be easily digested by analysts and technicians alike, with options to filter and explore the data over a timeline and within groups (oil pumps located within the same oil field). Ideally, there will be a customizable dashboard that shows crucial information at a glance. Fabrikam wants to know if there are out-of-the-box solutions for creating these visualizations and dashboards without involving developers and other IT staff.

Fabrikam has collected and compiled thorough maintenance and operational data of rod pump components and used this information to identify failure thresholds that identify an immediate or impending mechanical failure. Each of the elements of the rod pump's mechanism generates a consistent pattern in their telemetry when operating under normal conditions. If one of these components starts to degrade or suffers an operational failure, the signal reflects this change in status. When this change in status occurs, Fabrikam would like to automatically send an alert to engineers and field pump supervisors so they can potentially mitigate the issue and prevent damage to the failing and related components. In many cases, the fuel rod's onboard controller can modify the operating parameters of the pump to avoid or mitigate the impact of unexpected changes. Alternatively, if necessary, it can shut down the pump before any damage occurs and notify the company that repairs are necessary—protecting the machinery, and preventing potential environmental damage. Fabrikam would like to have a mechanism within their solution to send commands to rod pump controllers to modify these operating parameters remotely.

Their goal in the use of these monitoring capabilities and controls is to increase operator efficiency and safety. Addressing a typical maintenance issue takes several people and at least three days of system downtime at the cost of up to $20,000 USD a day, not including parts and labor. "By proactively identifying pump problems through automated monitoring, companies reduce unplanned downtime, which decreases costs, increases production, and increases the agility of maintenance services," says Fabrikam's Chief Technology Officer, Peter Guerin. He adds that the majority of industrial accidents don't happen at the well site; they happen when personnel is driving between sites. By eliminating the need for many site visits, they can reduce those accidents.

They would like to understand their options for expediting the implementation of the PoC. Specifically, they are looking to learn what offerings Azure provides that could enable a quick end-to-end start on the infrastructure for monitoring and managing devices and the system metadata. On top of this, they are curious about what other platform services Azure provides that they should consider in this scenario.

#### Telemetry data

Fabrikam identified 33 rod pump components whose telemetry they want to capture and monitor. Of these, they want to automatically monitor five with thresholds set on each that will trigger alerts if signals fall below those thresholds. According to their research and historical data for the rod pump's components, values that fall below these thresholds indicate either an impending or active failure of the pump.

| Field          | Type    | Normal measurement  | Failure threshold                     | Description                                                                                                                                                                                                                                                   |
| -------------- | ------- | ------------------- | ------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| MotorPowerkW   | Numeric | ~70kW               | ~30kW                                 | Measured in Kilowatts (kW); The power output of the motor should be steady, but will drop if there is a problem                                                                                                                                               |
| MotorSpeed     | Numeric | ~200 RPM            | ~80 RPM                               | Measured in RPM (including slip); Can change based on density of oil. Lower density causes this to go up (like if you have pockets of gas and it jumps up). If there's a failure it will go down below the normal operating average.                          |
| CasingFriction | Numeric | ~1450 psi           | ~830 psi                              | Measured in PSI (psi); The pressure will drop if a fissure is developing in the casing, indicating a failure.                                                                                                                                                 |
| PumpRate       | Numeric | ~60 SPM             | ~25 SPM                               | Speed calculated over the time duration between the last two times the crank arm has passed the proximity sensor measured in Strokes Per Minute (SPM) - minimum 0.0, maximum 100.0. A significant slow down could be an indicator of failure within the pump. |
| TimePumpOn     | Numeric | Amplitude: 0 - ~800 | Amplitude: 0 - ~340; higher frequency | Number of minutes the pump has been on. This should be a steady sawtooth pattern with a regular cadence of the pump cycling power. A consistently shorter than average cadence can indicate problems with the pump.                                           |

Normal signal readings from these five sensors appear in the chart below, with 10,000 intervals. The frequency of the output is condensed to fit:

![The five components' telemetry shown in their normal operating state.](media/normal-telemetry.png "Normal telemetry")

The next chart shows the telemetry for these five components during a gradual failure. In many cases, there is time to react and prevent total failure through remote commands to the pump controller:

![The five components' telemetry shown during gradual failure.](media/gradual-failure-telemetry.png "Gradual failure")

This final chart shows the telemetry for these five components when there is an immediate failure:

![The five components' telemetry during immediate failure.](media/immediate-failure-telemetry.png "Immediate failure")

### Customer needs

1. We are interested in comparing SaaS and PaaS-based offerings on Azure. Based on our needs, will a SaaS solution meet our needs for a rapid PoC?
2. Need web-based access to metadata management of the rod pump devices for simplified management by our users.
3. When we expand beyond the PoC, we need to be able to add devices in bulk for our customers.
4. We would like to have configurable standard dashboards for all users and personalized dashboards.
5. Need collection and export of real-time telemetry, at scale. There should be an intuitive interface for analyzing collected data as it arrives, or within timelines to include historical information.
6. Require strong device security with encryption at transit and at rest, as well as ample cloud-based security and user management.
7. We want to configure alerts when sensor data reaches certain thresholds so engineers can take action and send remote commands to pump controllers to prevent damage due to mechanical failure. Alert options should be flexible for choosing a destination and executing automated tasks.

### Customer objections

1. Is there an out of the box solution we can use to jump-start the creation of the solution?
2. We are worried about being constrained by a "black box" if we go with a SaaS solution. Do we have access to all of our collected telemetry we can use for external workloads?

### Infographic for common scenarios

Potential picture goes here...

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

1. Without getting into the details (the following sections will address the particular details), diagram your initial vision for using a SaaS-based IoT solution on Azure with device management, custom dashboards, user management, real-time telemetry capture, analysis, and export. If you can, include the underlying architecture of the SaaS solution by identifying its major components.

_IoT options in Azure_

1. What are the SaaS-based IoT options in Azure?

2. What are the PaaS-based IoT options in Azure?

3. Would you recommend SaaS or PaaS for this customer situation? What are the pros and cons of each?

_Device and metadata management_

1. How do you connect devices one at a time?

2. How do you connect multiple devices at scale? What options are there to secure device connections?

3. When Fabrikam is ready to mass manufacture devices, can they configure their devices to automatically connect to the cloud when turned on, or do they all have to be registered during installation?

4. What communication protocols are supported? What if they are using devices that do not support those protocols?

5. Is there a way to define common metadata for devices, such as location and serial number? How is this metadata applied to devices, and how can developers set this metadata programmatically?

6. How can control messages be sent to rod pump controllers from the cloud to perform tasks like turn off the pump engine or change settings?

_Dashboards and telemetry analysis_

1. How would you propose Fabrikam create visualizations for each rod pump? What options are available to view and filter the device telemetry?

2. How can they create shared dashboards, and can users create their own personalized dashboards?

3. Can telemetry be automatically exported to external storage for offline batch processing? What other options are available to gain access to telemetry outside of the core IoT solution?

_Security_

1. Is device data encrypted both in transit and at rest?

2. Can Fabrikam use standard certificates for device authentication? How do Fabrikam's administrators approve new devices that attempt to connect to the cloud?

3. What user management options are available for the dashboards? What roles are defined?

_Alerts and integrations_

1. Fabrikam wants to use their knowledge of rod pump component operating parameters and proactively monitor telemetry for immediate or impending failure. How can they set thresholds for sensor data and trigger alerts when those thresholds are crossed?

2. What options can they use to send alerts? They are interested in available integrations that may work with services they already use, like Office 365 or Dynamics CRM.

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

| Description                         | Links                                                                                                     |
| ----------------------------------- | --------------------------------------------------------------------------------------------------------- |
| About IoT Central                   | <https://docs.microsoft.com/en-us/azure/iot-central/overview-iot-central>                                 |
| About IoT Hub                       | <https://docs.microsoft.com/azure/iot-hub/iot-hub-what-is-iot-hub>                                        |
| What are IoT solution accelerators? | <https://docs.microsoft.com/en-us/azure/iot-accelerators/iot-accelerators-what-are-solution-accelerators> |