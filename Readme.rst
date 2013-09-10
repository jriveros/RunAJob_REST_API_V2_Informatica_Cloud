Welcome
=============

Using RunAJob for Windows environment. (Developed in C#)
Description:
This Console program is used to schedule Informatica Cloud task from windows environment, Is currently in Beta, and under heavy improvements.
Informatica Cloud provide and REST API v2.0 which is used for remote call/response for many types of calls. For this is being used to remote schedule task, and check if the task actually run or not.


Setup and Configuration:
=========================
•	Download the and unzip file into your preferred directory. (C:\Temp)
•	In the directory you will found 3 files 
	o	RunAJob.exe : Is the console executable which actually connect to ICS
	o	 InitialProperties.xml : XML which contain the initial properties.
	o	bat.bat : A bat file example.
•	Provide your credential and server url information in InitialProperties.xml

 
	<doDebug> is being used to return a simple debug information about where it being in the process to schedule a task.  Example output:
 
•	This utility runs a given task (job) on Informatica Cloud. It requires the following information:
example:
RunAJob <TaskId> <TaskName> <TaskType> <[doActivityCheck] >
where:
o	<Task Id>: This is the ID of the task as it shows up on the URL when you open the task on the UI. It's the last alphanumeric value in the URL.  
o	<TaskName> : Used for check the task against ActivityMonitor and ActivityLog API.
o	<TaskType> : could be DSS or Workflow, depending the task type.
o	[doActivityCheck] = <true|false>, in case you want to check and wait for the task run.
If you just want to fire and drop set up in false.
If you want to wait set up in true.
 
•	Open and modify the bat.bat file
 
•	Return Value (%ERRORLEVEL%) - The utility returns an integer value indicating the status of the run:
Value	Status
1	Success
2	With Error
3	Failed
999	Other Error.


Special Features:
========================
•	It retry 10 times to log into ICS, then it will  stop after 50 retries with error 999.
•	It retry 10 times to schedule a task, then it will  stop after 50 retries with error 999
•	If the task has been scheduled and could not confirm in ActivityMonitor it retry for ActivityMonitor 100 times (remote calls), then stop and will check in ActivityLog.
•	When is checking in activity log, it get the first 50 entries, so if many task run at the same time, and it get the entry 51, then it will  stop after 50 retries with error 999
