#include "HelloWorld.h"

namespace Hoge
{
     static int staticIntArray[8] = {0, 0, 0, 0, 1, 1, 1, 1};

     float FooPluginFunction()
     {
          return 5.0f;
     }

     void GetByteArray(unsigned int *ptr)
     {
          unsigned int array[8] = {0, 1, 2, 3, 4, 5, 6, 7};
          ptr = array;
     }

     void GetIntArray(int *ptr)
     {
          for (int i = 0; i < 8; i++)
          {
               ptr[i] = i + 1;
          }
     }
}
