## Flash Tool

This tool simplifies the firmware installation process, making it accessible even for users without extensive technical background.

### README Translation Version

---

- [English](README.md)
- [中文](README.zh.md)

### Getting Started

---

The following instructions will help you get a copy of the project on your local machine and set it up for development and testing.

### Prerequisites

---

You need to install the following software and ensure they are correctly installed:

- Windows 10 version 1803 or later.
- .NET Framework 4.7.2 or later must be installed on your machine.

### Installation Steps

---

The following step-by-step example will guide you on how to set up your development environment:

1. Clone the repository or download the ZIP file.

```bash
git clone https://github.com/Ranshen1209/Ranshen-s-Flash.git
```

2. Navigate to the cloned or downloaded directory.

3. Build the project using Visual Studio.

4. After building the project, the `flashbin` directory will be configured with the necessary files and folders for operations.

```bash
/flashbin
|-- /platform-tools           # Contains Google's adb and fastboot executables
|-- /resources                # Content required for program operation
|-- /drivers                  # Contains device drivers, particularly those from Xiaomi
```

### How to Use

---

#### Driver Installation Script Example

```bash
@echo off
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Google\Driver\android_winusb.inf" /install
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Qualcomm\Driver\qcser.inf" /install
```

#### Fastboot Script Example

```bash
@echo off
set PATH=%~dp0flashbin\platform-tools;%PATH%

REM Check device model
fastboot %* getvar product 2>&1 | findstr /r /c:"^product: *marble" || (
    echo Missmatching image and device
    exit /B 1
)

REM Erase boot_ab partition
fastboot %* erase boot_ab || (
    echo "Erase boot error"
    exit /B 1
)

REM Define images and their corresponding partitions to flash
set images=abl_ab xbl_ab xbl_config_ab shrm_ab aop_ab aop_config_ab tz_ab devcfg_ab featenabler_ab hyp_ab uefi_ab uefisecapp_ab modem_ab bluetooth_ab dsp_ab keymaster_ab qupfw_ab cpucp_ab rescue xbl_ramdump_ab imagefv_ab vendor_boot_ab dtbo_ab vbmeta_ab vbmeta_system_ab cust recovery_ab boot_ab
set files=abl.img xbl.img xbl_config.img shrm.img aop.img aop_config.img tz.img devcfg.img featenabler.img hyp.img uefi.img uefisecapp.img modem.img bluetooth.img dsp.img keymaster.img qupfw.img cpucp.img rescue.img xbl_ramdump.img imagefv.img vendor_boot.img dtbo.img vbmeta.img vbmeta_system.img cust.img recovery.img boot.img

REM Loop to flash images
for %%i in (%images%) do (
    for %%j in (%files%) do (
        fastboot %* flash %%i %~dp0images/%%j || (
            echo "Flash %%i error"
            exit 1
        )
    )
)

REM Erase and flash imagefv_ab partition
fastboot %* erase imagefv_ab || (
    echo "Erase imagefv error"
    exit /B 1
)
fastboot %* flash imagefv_ab %~dp0images/imagefv.img || (
    echo "Flash imagefv error"
    exit 1
)

REM Flash super.img and misc.img separately
fastboot %* flash super %~dp0super.img || (
    echo "Flash super error"
    exit 1
)
fastboot %* flash misc %~dp0images/misc.img || (
    echo "Flash misc error"
    exit /B 1
)

REM Flash vbmeta.img and vbmeta_system.img with verity and verification disabled
fastboot %* --disable-verity --disable-verification flash vbmeta %~dp0images/vbmeta.img || (
    echo "Flash vbmeta error"
    exit 1
)
fastboot %* --disable-verity --disable-verification flash vbmeta_system %~dp0images/vbmeta_system.img || (
    echo "Flash vbmeta_system error"
    exit 1
)

REM Set active partition to a
fastboot %* set_active a || (
    echo "Set active a error"
    exit 1
)

REM Reboot device
fastboot %* reboot || (
    echo "Reboot error"
    exit 1
)
```

#### TWRP Flash Script Example

```shell
#!/sbin/sh
set -e
ZIP=$3
OUTFD=/proc/self/fd/$2

ui_print(){
    echo -e "ui_print $1\nui_print" > $OUTFD
}

package_extract_file(){
    if unzip -l "$ZIP" "$1" >/dev/null 2>&1; then
        unzip -p "$ZIP" "$1" > "$2"
    else
        ui_print "Error: Cannot find $1 in $ZIP"
        exit 1
    fi
}

PartitionFilePath=/dev/block/by-name

flash_partition() {
    local partition=$1
    ui_print "Flashing $partition partition"
    package_extract_file "images/$partition.img" "${PartitionFilePath}/${partition}_a"
    package_extract_file "images/$partition.img" "${PartitionFilePath}/${partition}_b"
}

for partition in abl xbl xbl_config shrm aop aop_config tz devcfg featenabler hyp uefi uefisecapp modem bluetooth dsp keymaster qupfw cpucp xbl_ramdump imagefv vendor_boot dtbo vbmeta vbmeta_system recovery; do
    flash_partition $partition
done

ui_print "Flashing rescue partition"
package_extract_file "images/rescue.img" "${PartitionFilePath}/rescue"

ui_print "Flashing super partition"
package_extract_file "super.img" "${PartitionFilePath}/super"

ui_print "Flashing cust partition"
package_extract_file "images/cust.img" "${PartitionFilePath}/cust"

ui_print "Flashing misc partition"
package_extract_file "images/misc.img" "${PartitionFilePath}/misc"

exit 0
```