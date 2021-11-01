_Changes for v 12.4_:
- App now able to add versions of successfully deployed packages to the registry. It allows you to skip deployment package in the next DPModule start without starting the deployed project (was required in previous version). Also, this option allows you to track versions of those projects that cannot check them on their own (KeyboardSwitcher, ESHQ, etc.);
- DPModule now able to filter packages with “only new” and “only updates” flags;
- Updated internal mechanism of checking for updates (applied compatibility with new GitHub theme);
- Added the registry access checker: it will show recommended actions for the executable file when Windows doesn’t allow saving settings;
- AboutForm and HardWorkExecutor universal classes have been upgraded and improved
