![](https://github.com/Microsoft/MCW-Template-Cloud-Workshop/raw/master/Media/ms-cloud-workshop.png "Microsoft Cloud Workshops")

<div class="MCWHeader1">
Predictive Maintenance for Remote Field Devices
</div>

<div class="MCWHeader2">
Hands-on lab step-by-step
</div>

<div class="MCWHeader3">
July 2019
</div>


Information in this document, including URL and other Internet Web site references, is subject to change without notice. Unless otherwise noted, the example companies, organizations, products, domain names, e-mail addresses, logos, people, places, and events depicted herein are fictitious, and no association with any real company, organization, product, domain name, e-mail address, logo, person, place or event is intended or should be inferred. Complying with all applicable copyright laws is the responsibility of the user. Without limiting the rights under copyright, no part of this document may be reproduced, stored in or introduced into a retrieval system, or transmitted in any form or by any means (electronic, mechanical, photocopying, recording, or otherwise), or for any purpose, without the express written permission of Microsoft Corporation.

Microsoft may have patents, patent applications, trademarks, copyrights, or other intellectual property rights covering subject matter in this document. Except as expressly provided in any written license agreement from Microsoft, the furnishing of this document does not give you any license to these patents, trademarks, copyrights, or other intellectual property.

The names of manufacturers, products, or URLs are provided for informational purposes only and Microsoft makes no representations and warranties, either expressed, implied, or statutory, regarding these manufacturers or the use of the products with any Microsoft technologies. The inclusion of a manufacturer or product does not imply endorsement of Microsoft of the manufacturer or product. Links may be provided to third party sites. Such sites are not under the control of Microsoft and Microsoft is not responsible for the contents of any linked site or any link contained in a linked site, or any changes or updates to such sites. Microsoft is not responsible for webcasting or any other form of transmission received from any linked site. Microsoft is providing these links to you only as a convenience, and the inclusion of any link does not imply endorsement of Microsoft of the site or the products contained therein.

© 2018 Microsoft Corporation. All rights reserved.

Microsoft and the trademarks listed at <https://www.microsoft.com/en-us/legal/intellectualproperty/Trademarks/Usage/General.aspx> are trademarks of the Microsoft group of companies. All other trademarks are property of their respective owners.

**Contents** 

<!-- TOC -->

- Predictive Maintenance for Remote Field Devices hands-on lab step-by-step](#predictive-maintenance-for-remote-field-devices-hands-on-lab-step-by-step)
    - [Abstract and learning objectives](#abstract-and-learning-objectives)
    - [Overview](#overview)
    - [Solution architecture](#solution-architecture)
    - [Requirements](#requirements)
    - [Before the hands-on lab](#before-the-hands-on-lab)
    - [Exercise 1: Configuring IoT Central with devices and metadata](#exercise-1-configuring-iot-central-with-devices-and-metadata)
        - [Task 1: Task name](#task-1-task-name)
        - [Task 2: Task name](#task-2-task-name)
    - [Exercise 2: Exercise name](#exercise-2-exercise-name)
        - [Task 1: Task name](#task-1-task-name-1)
        - [Task 2: Task name](#task-2-task-name-1)
    - [Exercise 3: Exercise name](#exercise-3-exercise-name)
        - [Task 1: Task name](#task-1-task-name-2)
        - [Task 2: Task name](#task-2-task-name-2)
    - [After the hands-on lab](#after-the-hands-on-lab)
        - [Task 1: Task name](#task-1-task-name-3)
        - [Task 2: Task name](#task-2-task-name-3)

<!-- /TOC -->

# Predictive Maintenance for Remote Field Devicesmhands-on lab step-by-step 

## Abstract and learning objectives 

In this hands-on-lab, you will build an end-to-end industrial IoT solution. We will begin by leveraging the Azure IoT Central SaaS offerings to quickly stand up a fully functional remote monitoring solution. Azure IoT Central provides solutions built upon recommendations found in the [Azure IoT Reference Architecture](https://docs.microsoft.com/en-us/azure/architecture/reference-architectures/iot/). We will customize this system specifically for rod pumps. Rod pumps are industrial equipment that is heavily used in the oil and gas industry.  We will then establish a model for the telemetry data that is received from the pump systems in the field and use this model to deploy simulated devices for system testing purposes. 

Furthermore we will establish threshold rules in the remote monitoring system that will monitor the incoming telemetry data to ensure all equipment is running optimally, and can alert us whenever the equipment is running outside of normal boundaries - indicating the need for alternative running parameters, maintenance, or a complete shut down of the pump. By leveraging the IoT Central solution, users can also issue commands to the pumps from a remote location in an instant to automate many operational and maintenance tasks which used to require staff on-site. This lessens operating costs associated with technician dispatch and equipment damage due to a failure.

Above and beyond real-time monitoring and mitigating immediate equipment damage through commanding - you will also learn how to apply the historical telemetry data accumulated to identify positive and negative trends that can be used to adjust daily operations for higher throughput and reliability.

TODO: AI/ML abstract

## Overview

The Predictive Maintenance for Remote Field Devices hands-on lab is an exercise that will challenge you to implement an end-to-end scenario using the supplied example that is based on Azure IoT Central and other related Azure services. The hands-on lab can be implemented on your own, but it is highly recommended to pair up with other members at the lab to model a real-world experience and to allow each member to share their expertise for the overall solution.

## Solution architecture

\[Insert your end-solution architecture here. . .\]

## Requirements

1.  Number and insert your custom workshop content here . . . 

## Before the hands-on lab

Refer to the Before the hands-on lab setup guide manual before continuing to the lab exercises.

To author: remove this section if you do not require environment setup instructions.

## Exercise 1: Configuring IoT Central with devices and metadata

Duration: X minutes

\[insert your custom Hands-on lab content here . . . \]

#### Telemetry Schema
| Field                | Type     | Description                                                                                                                                                                                                                                                                    |
|----------------------|----------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| SerialNumber         | String   | Unique serial number identifying the rod pump equipment                                                                                                                                                                                                                        |
| IPAddress            | String   | Current IP Address                                                                                                                                                                                                                                                             |
| TimeStamp            | DateTime | Timestamp in UTC identifying the point in time the telemetry was created                                                                                                                                                                                                       |
| SPMtoHzRatio         | Numeric  | Ratio of crank arm SPM to drive output frequency in Hz. Unit (SPM/Hz)                                                                                                                                                                                                          |
| PumpRate             | Numeric  | Speed calculated over the time duration between the last two times the crank arm has passed the proximity sensor measured in Strokes Per Minute (SPM) - minimum 0.0, maximum 100.0                                                                                             |
| FluidHeightCasing    | Numeric  | Measured in Feet (ft)                                                                                                                                                                                                                                                          |
| PumpIntakePressure   | Numeric  | Measured in Pounds per Square Inch (psi)                                                                                                                                                                                                                                       |
| FluidLoad            | Numeric  | Measured in Pounds (lbs)                                                                                                                                                                                                                                                       |
| TimePumpOn           | Numeric  | Number of minutes the pump has been on                                                                                                                                                                                                                                         |
| BeltSlip             | Numeric  | The percentage difference between the speed measured by the drive (motor speed) and the pump speed measured by the proximity sensor (crank arm speed), Units (%) - minimum -1000.0, maximum +1,000.0   (est. alerm threshold 8%)                                                                        |
| Imbalance            | Numeric  | The percentage difference between the counter weight and the rod string weight (maximum upstroke torque and the maximum downstroke torque). A negative value indicates the counterbalance weight is greater than the rod strength. Units (%), minimum -1000.0, maximum +1000.0 - Counterbalance = 100% * (Maximum Upstroke Torque - Maximum Downstroke Torque) / Rated Torque |
| LoadCell             | Numeric  | Load cell reading in lbs, minimum 0, maximum 60,000                                                                                                                                                                                                                            |
| MotorCurrent         | Numeric  | Measured in Amps (A)                                                                                                                                                                                                                                                           |
| MotorVoltage         | Numeric  | Measured in Volts (V)                                                                                                                                                                                                                                                          |
| MotorPowerHP         | Numeric  | Measured in Horse Power (HP)                                                                                                                                                                                                                                                   |
| MotorPowerkW         | Numeric  | Measured in Kilowatts (kW)                                                                                                                                                                                                                                                     |
| MotorSpeed           | Numeric  | including slip (RPM)                                                                                                                                                                                                                                                           |
| PolishedRodPower | Numeric | Peak polished rod horsepower (HP) |
| PumpPower | Numeric | Measured in horsepower (HP) |
| EnergyConsumption    | Numeric  | Measured in Kilowatt Hours (kWh)                                                                                                                                                                                                                                               |
| LoadCellForce        | Numeric  | Measured in Pounds (lbs)                                                                                                                                                                                                                                                       |
| DriveOutputTorque    | Numeric  | Torque Output (%) (est. min. 1.8, target 18.0, threshold high 300%)                                                                                                                                                                                                                                                             |
| StuffingBoxFriction  | Numeric  | Friction load from the stuffing box (lbs)                                                                                                                                                                                                                                      |
| TubingFriction       | Numeric  | Measured in PSI (psi)                                                                                                                                                                                                                                                          |
| CasingFriction       | Numeric  | Measured in PSI (psi)                                                                                                                                                                                                                                                          |
| DriveOutputFrequency | Numeric  | Measured in Hertz (Hz)                                                                                                                                                                                                                                                         |
| DriveOutputCurrent   | Numeric  | Measured in Amps (A)                                                                                                                                                                                                                                                           |
| FillPercentDownhole | Numeric | Downhole fillage in percent (%) (est. min. 10.0, target 150.0) |
| FillPercentSurface | Numeric | Surface fillage in percent (%) (est. min. 10.0, target 121.0) |
| StrokeLengthSurface | Numeric | Measured in inches (in) |
| StrokeLengthDownhole | Numeric | Measured in inches (in) |
| LoadCellSensorAvailable | Bool | True if the sensor is responsive, false otherwise |
| ProximitySensorAvailable | Bool | True if the sensor is responsive, false otherwise |

[reference](https://download.schneider-electric.com/files?p_enDocType=User+guide&p_File_Name=Realift+RPC+plus+Config+Manual.pdf&p_Doc_Ref=Realift_RPC_Config_Manual)

### Task 1: Task name

1.  Number and insert your custom workshop content here . . . 

    a.  Insert content here

        i.  

### Task 2: Task name

1.  Number and insert your custom workshop content here . . . 

    a.  Insert content here

        i.  


## Exercise 2: Exercise name

Duration: X minutes

\[insert your custom Hands-on lab content here . . . \]

### Task 1: Task name

1.  Number and insert your custom workshop content here . . . 

    a.  Insert content here

        i.  

### Task 2: Task name

1.  Number and insert your custom workshop content here . . . 

    a.  Insert content here

        i.  


## Exercise 3: Exercise name

Duration: X minutes

\[insert your custom Hands-on lab content here . . .\]

### Task 1: Task name

1.  Number and insert your custom workshop content here . . .

    a.  Insert content here

        i.  

### Task 2: Task name

1.  Number and insert your custom workshop content here . . .

    a.  Insert content here

        i.  

## After the hands-on lab 

Duration: X minutes

\[insert your custom Hands-on lab content here . . .\]

### Task 1: Task name

1.  Number and insert your custom workshop content here . . .

    a.  Insert content here

        i.  

### Task 2: Task name

1.  Number and insert your custom workshop content here . . .

    a.  Insert content here

        i.  
You should follow all steps provided *after* attending the Hands-on lab.

