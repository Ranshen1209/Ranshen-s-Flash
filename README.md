## Flashing Tool

This tool simplifies the process of installing new firmware, making it accessible even for users without extensive technical background.

### README Translated Versions

---

- [English](README.md)
- [中文](README.zh.md)

### Getting Started

---

The following instructions will help you obtain a copy of the project on your local machine and prepare it for development and testing.

### Prerequisites

---

You need to install the following software and ensure they are correctly installed:

- Windows 10 version 1803 or later.
- .NET Framework 4.7.2 or later must be installed on your machine.

### Installation Steps

---

The step-by-step examples below will guide you on how to set up the development environment:

1. Clone the repository or download the ZIP file.

```bash
git clone https://github.com/Ranshen1209/Ranshen-s-Flash.git
```

2. Navigate to the cloned or downloaded directory.

3. Build the project using Visual Studio.

4. After building the project, the `flashbin` directory will be configured with the necessary files and folders for operation.

```bash
/flashbin
|-- /platform-tools           # Contains Google's adb and fastboot executables
|-- /resources                # Contents required for the program to run
|-- /drivers                  # Contains device drivers, particularly from Xiaomi
```

### How to Apply

---

#### Driver Installation Script Example

```bash
@echo off
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Google\Driver\android_winusb.inf" /install
"C:\Windows\sysnative\pnputil.exe" /add-driver "%~dp0..\ThirdParty\Qualcomm\Driver\qcser.inf" /install
```

#### Line Flashing Script Example

```bash
@echo off
setlocal
set PATH=%~dp0flashbin\platform-tools;%~dp0flashbin\zstd;%PATH%

REM Check device model
for /f "tokens=2 delims=: " %%a in ('fastboot %* getvar product 2^>^&1 ^| findstr /r /c:"^product: *marble"') do (
    if /i not "%%a"=="marble" (
        echo Missmatching image and device
        exit /B 1
    )
)

REM Ensure %~dp0 does not end with a backslash
set scriptDir=%~dp0
if "%scriptDir:~-1%"=="\" set scriptDir=%scriptDir:~0,-1%

REM Check and remove existing super.img if exists
if exist "%scriptDir%\images\super.img" (
    echo Removing existing super.img
    del "%scriptDir%\images\super.img" || (
        echo "Failed to delete existing super.img"
        exit /B 1
    )
)

REM Decompress the super.img.zst file to the super.img file
echo Decompressing super.img.zst to super.img
zstd -d "%scriptDir%\super.img.zst" -o "%scriptDir%\images\super.img" || (
    echo "Decompression error"
    exit /B 1
)

REM Flash super.img
echo Flashing super partition
fastboot %* flash super "%scriptDir%\images\super.img" || (
    echo "Flash super error"
    exit /B 1
)

REM Erase boot_ab partition
echo Erasing boot_ab partition
fastboot %* erase boot_ab || (
    echo "Erase boot error"
    exit /B 1
)

REM Flash individual partitions
echo Flashing abl_ab
fastboot %* flash abl_ab "%scriptDir%\images\abl.img" || (
    echo "Flash abl_ab error"
    exit /B 1
)

echo Flashing xbl_ab
fastboot %* flash xbl_ab "%scriptDir%\images\xbl.img" || (
    echo "Flash xbl_ab error"
    exit /B 1
)

echo Flashing xbl_config_ab
fastboot %* flash xbl_config_ab "%scriptDir%\images\xbl_config.img" || (
    echo "Flash xbl_config_ab error"
    exit /B 1
)

echo Flashing shrm_ab
fastboot %* flash shrm_ab "%scriptDir%\images\shrm.img" || (
    echo "Flash shrm_ab error"
    exit /B 1
)

echo Flashing aop_ab
fastboot %* flash aop_ab "%scriptDir%\images\aop.img" || (
    echo "Flash aop_ab error"
    exit /B 1
)

echo Flashing aop_config_ab
fastboot %* flash aop_config_ab "%scriptDir%\images\aop_config.img" || (
    echo "Flash aop_config_ab error"
    exit /B 1
)

echo Flashing tz_ab
fastboot %* flash tz_ab "%scriptDir%\images\tz.img" || (
    echo "Flash tz_ab error"
    exit /B 1
)

echo Flashing devcfg_ab
fastboot %* flash devcfg_ab "%scriptDir%\images\devcfg.img" || (
    echo "Flash devcfg_ab error"
    exit /B 1
)

echo Flashing featenabler_ab
fastboot %* flash featenabler_ab "%scriptDir%\images\featenabler.img" || (
    echo "Flash featenabler_ab error"
    exit /B 1
)

echo Flashing hyp_ab
fastboot %* flash hyp_ab "%scriptDir%\images\hyp.img" || (
    echo "Flash hyp_ab error"
    exit /B 1
)

echo Flashing uefi_ab
fastboot %* flash uefi_ab "%scriptDir%\images\uefi.img" || (
    echo "Flash uefi_ab error"
    exit /B 1
)

echo Flashing uefisecapp_ab
fastboot %* flash uefisecapp_ab "%scriptDir%\images\uefisecapp.img" || (
    echo "Flash uefisecapp_ab error"
    exit /B 1
)

echo Flashing modem_ab
fastboot %* flash modem_ab "%scriptDir%\images\modem.img" || (
    echo "Flash modem_ab error"
    exit /B 1
)

echo Flashing bluetooth_ab
fastboot %* flash bluetooth_ab "%scriptDir%\images\bluetooth.img" || (
    echo "Flash bluetooth_ab error"
    exit /B 1
)

echo Flashing dsp_ab
fastboot %* flash dsp_ab "%scriptDir%\images\dsp.img" || (
    echo "Flash dsp_ab error"
    exit /B 1
)

echo Flashing keymaster_ab
fastboot %* flash keymaster_ab "%scriptDir%\images\keymaster.img" || (
    echo "Flash keymaster_ab error"
    exit /B 1
)

echo Flashing qupfw_ab
fastboot %* flash qupfw_ab "%scriptDir%\images\qupfw.img" || (
    echo "Flash qupfw_ab error"
    exit /B 1
)

echo Flashing cpucp_ab
fastboot %* flash cpucp_ab "%scriptDir%\images\cpucp.img" || (
    echo "Flash cpucp_ab error"
    exit /B 1
)

echo Flashing rescue
fastboot %* flash rescue "%scriptDir%\images\rescue.img" || (
    echo "Flash rescue error"
    exit /B 1
)

echo Flashing xbl_ramdump_ab
fastboot %* flash xbl_ramdump_ab "%scriptDir%\images\xbl_ramdump.img" || (
    echo "Flash xbl_ramdump_ab error"
    exit /B 1
)

echo Flashing imagefv_ab
fastboot %* flash imagefv_ab "%scriptDir%\images\imagefv.img" || (
    echo "Flash imagefv_ab error"
    exit /B 1
)

echo Flashing vendor_boot_ab
fastboot %* flash vendor_boot_ab "%scriptDir%\images\vendor_boot.img" || (
    echo "Flash vendor_boot_ab error"
    exit /B 1
)

echo Flashing dtbo_ab
fastboot %* flash dtbo_ab "%scriptDir%\images\dtbo.img" || (
    echo "Flash dtbo_ab error"
    exit /B 1
)

echo Flashing vbmeta_ab
fastboot %* flash vbmeta_ab "%scriptDir%\images\vbmeta.img" || (
    echo "Flash vbmeta_ab error"
    exit /B 1
)

echo Flashing vbmeta_system_ab
fastboot %* flash vbmeta_system_ab "%scriptDir%\images\vbmeta_system.img" || (
    echo "Flash vbmeta_system_ab error"
    exit /B 1
)

echo Flashing cust
fastboot %* flash cust "%scriptDir%\images\cust.img" || (
    echo "Flash cust error"
    exit /B 1
)

echo Flashing recovery_ab
fastboot %* flash recovery_ab "%scriptDir%\images\recovery.img" || (
    echo "Flash recovery_ab error"
    exit /B 1
)

echo Flashing boot_ab
fastboot %* flash boot_ab "%scriptDir%\images\boot.img" || (
    echo "Flash boot_ab error"
    exit /B 1
)

REM Erase and flash imagefv_ab partition
echo Erasing imagefv_ab partition
fastboot %* erase imagefv_ab || (
    echo "Erase imagefv error"
    exit /B 1
)
echo Flashing imagefv_ab partition
fastboot %* flash imagefv_ab "%scriptDir%\images\imagefv.img" || (
    echo "Flash imagefv error"
    exit /B 1
)

REM Flash misc partition
echo Flashing misc partition
fastboot %* flash misc "%scriptDir%\images\misc.img" || (
    echo "Flash misc error"
    exit /B 1
)

REM Flash vbmeta.img and vbmeta_system.img with verity and verification disabled
echo Flashing vbmeta partition with verity and verification disabled
fastboot %* --disable-verity --disable-verification flash vbmeta "%scriptDir%\images\vbmeta.img" || (
    echo "Flash vbmeta error"
    exit /B 1
)
echo Flashing vbmeta_system partition with verity and verification disabled
fastboot %* --disable-verity --disable-verification flash vbmeta_system "%scriptDir%\images\vbmeta_system.img" || (
    echo "Flash vbmeta_system error"
    exit /B 1
)

REM Set active partition to a
echo Setting active partition to a
fastboot %* set_active a || (
    echo "Set active a error"
    exit /B 1
)

REM Remove the decompressed super.img file if exists
if exist "%scriptDir%\images\super.img" (
    echo Removing decompressed super.img file
    del "%scriptDir%\images\super.img" || (
        echo "Failed to delete super.img"
        exit /B 1
    )
)

REM Reboot device
echo Rebooting device
fastboot %* reboot || (
    echo "Reboot error"
    exit /B 1
)

endlocal
exit 0
```

#### Card Flashing Script Example

```shell
#!/sbin/bash

OUTFD=/proc/self/fd/$2
ZIPFILE="$3"
ZIPNAME=${ZIPFILE##*/}

[ -d /tmp ] && rm -rf /tmp
mkdir -p /tmp

unzip "$ZIPFILE" flashbin/zstd/zstd -d /tmp
chmod -R 0755 /tmp

ui_print(){
    echo -e "ui_print $1\nui_print" > $OUTFD
}

package_extract_file() {
    unzip -p "$ZIPFILE" $1 >$2
}

package_extract_zstd() {
    unzip -p "$ZIPFILE" $1 | /tmp/flashbin/zstd/zstd -k -d >$2
}

PartitionFilePath=/dev/block/bootdevice/by-name/

flash_partition() {
    local partition=$1
    ui_print "Flashing $partition partition"
    package_extract_file "images/$partition.img" "${PartitionFilePath}/${partition}_a"
    package_extract_file "images/$partition.img" "${PartitionFilePath}/${partition}_b"
}

ui_print " "
ui_print "$ZIPNAME"
ui_print "Produced by Ranshen."
ui_print " "

for partition in abl xbl xbl_config shrm aop aop_config tz devcfg featenabler hyp uefi uefisecapp modem bluetooth dsp keymaster qupfw cpucp xbl_ramdump imagefv vendor_boot dtbo vbmeta vbmeta_system recovery; do
    flash_partition "$partition"
done

ui_print "Flashing super partition"
package_extract_zstd "super.img.zst" "${PartitionFilePath}/super"

ui_print "Flashing cust partition"
package_extract_file "images/cust.img" "${PartitionFilePath}/cust"

ui_print "Flashing logo partition"
package_extract_file "images/logo.img" "${PartitionFilePath}/logo"

ui_print "Clearing cache."
rm -rf /data/cache/
rm -rf /data/dalvik-cache/
rm -rf /data/system/package_cache/
[ -d /tmp ] && rm -rf /tmp

ui_print " "
ui_print "Flashing completed."
exit 0
```