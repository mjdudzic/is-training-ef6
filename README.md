# is-training-ef6

Repository to store code and files for ef6 training

# Prerequisites

## Prepare trainig database

For the training we will use StackOverflow2010 database which is available online.
There are to approaches to prepare the database

### Download already prepared database

Database files (7z) with proper schema required for training purpose file can be downloaded from xxx

It is the database downloaded from https://www.brentozar.com/archive/2015/10/how-to-download-the-stack-overflow-database-via-bittorrent/ with some data and schema fix alrady provided.

Attach the files to your SQL Server instnace.

_If you encounter issue with Access denied during db attach just add your account (you used to log in into system) with all privileges to the db files: mdf and ldf
Add permissions in file Properties window in Security tab_

### Download from origin

Download the 10 GB database (7z) from https://www.brentozar.com/archive/2015/10/how-to-download-the-stack-overflow-database-via-bittorrent/

Attach it to your SQL Server
Follow instructions from README.md in the downloaded 7z file.

_If you encounter issue with Access denied during db attach just add your account (you used to log in into system) with all privileges to the db files: mdf and ldf
Add permissions in file Properties window in Security tab_

Next following scripts must be executed for the attached database `[respository path]/db/fix_scripts`:

- data_fix.sql
- schema_fix.sql

These scripts provide data and schema updates
