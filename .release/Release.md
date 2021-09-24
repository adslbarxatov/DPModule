_Changes for v 12.0.1_:
- Disabled the post-deployment relocation for packages; “Deployed” directory is no longer used;
- Disabled support for .dp9 packages format;
- Fixed some interface bugs;
- Interface of packages interface has been completely rebuilt. Now it has obviously display actual and installed versions for packages, load project descriptions and correctly process user selection for further deployment;
- DPModule now allows users to skip creation of desktop shortcuts;
- Now DPModule has its own type for deinstallation scripts. These files (.dpd) are copies of deployment packages (.dp0) that contain only the header (without product data)
