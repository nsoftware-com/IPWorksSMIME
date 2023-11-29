/*
 * IPWorks S/MIME 2022 C++ Edition - Sample Project
 *
 * This sample project demonstrates the usage of IPWorks S/MIME in a 
 * simple, straightforward way. It is not intended to be a complete 
 * application. Error handling and other checks are simplified for clarity.
 *
 * www.nsoftware.com/ipworkssmime
 *
 * This code is subject to the terms and conditions specified in the 
 * corresponding product license agreement which outlines the authorized 
 * usage and restrictions.
 */

#include <stdio.h>
#include <stdlib.h>
#include <cstring>
#include "../../include/ipworkssmime.h"
#define LINE_LEN 100

class MyCertMgr : public CertMgr
{
	//overwrite events here if needed
public:
	virtual int FireCertList(CertMgrCertListEventParams *e)
	{
		printf("%s\n", e->CertSubject);
		return 0;
	}
};

int main(int argc, char **argv)
{
	char * certStore = "./test.pfx";
	int certStoreSize;
	MyCertMgr certmgr;
	
	certmgr.SetCertStoreType(2); //CST_PFXFILE
	certmgr.SetCertStore(certStore, strlen(certStore));
	certmgr.SetCertStorePassword("test");
	certmgr.GetCertStore(certStore, certStoreSize);
	printf("Listing all certificates in store %s:\n\n", certStore);

	certmgr.ListStoreCertificates();
	if (certmgr.GetLastErrorCode())
	{
		printf("%d (%s)", certmgr.GetLastErrorCode(), certmgr.GetLastError());
	}
	printf("\npress <return> to continue...\n");
	getchar();
}


