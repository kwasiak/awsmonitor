# awsmonitor
This is small Windows application, which can help you with tracking what kind of resources are active in your AWS account. 

# What resources does it track?
At the moment tracking is limited to following types of AWS resources:
- EC2 instances
- EBS disk volumes (both: attached and detached from EC2 instances)
- Security groups
- RDS databases
- Elastic Beanstalk instances

# Additional tools for EC2 instances
For each EC2 instance you can:
- start and stop it ('Action' column)
- run Remote Desktop Session (RDP in 'Tools' column) - works ONLY for Windows instances, which have RDP enabled
- run default Web Browser with root URL of the instance

If you want to see more details related to EC2 instance, you can do that by double-clicking single row with EC2 details. It will show you security groups defined for that particular EC2 instance and details of how each single group is defined (as: what incoming and outgoing traffic is enabled).
