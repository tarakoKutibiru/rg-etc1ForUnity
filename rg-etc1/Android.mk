LOCAL_PATH := $(call my-dir)

include $(CLEAR_VARS)

LOCAL_MODULE    := rg_etc1
LOCAL_MODULE_FILENAME := libRg_etc1
LOCAL_CFLAGS    := -Werror
CPP_FILES := $(shell find $(LOCAL_PATH)/src -name *.cpp)
CPP_FILES += $(shell find $(LOCAL_PATH)/src -name *.c)
LOCAL_SRC_FILES += $(CPP_FILES:$(LOCAL_PATH)/%=%)
LOCAL_C_INCLUDES := $(LOCAL_PATH)/src
LOCAL_LDLIBS    := -llog

include $(BUILD_SHARED_LIBRARY)