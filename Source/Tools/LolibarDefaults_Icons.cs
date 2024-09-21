﻿using System.Windows.Media;

namespace LolibarApp.Source.Tools
{
    public partial class LolibarDefaults
    {
        public static readonly Geometry? CurProcIcon            = Geometry.Parse("M6 0C6.37877 0 6.72504 0.214002 6.89443 0.552786L10 6.76393L11.1056 4.55279C11.275 4.214 11.6212 4 12 4H15C15.5523 4 16 4.44772 16 5C16 5.55228 15.5523 6 15 6H12.618L10.8944 9.44721C10.725 9.786 10.3788 10 10 10C9.62123 10 9.27497 9.786 9.10557 9.44721L6 3.23607L4.89443 5.44721C4.72504 5.786 4.37877 6 4 6H1C0.447715 6 0 5.55228 0 5C0 4.44772 0.447715 4 1 4H3.38197L5.10557 0.552786C5.27496 0.214002 5.62123 0 6 0Z");
        public static readonly Geometry? CpuIcon                = Geometry.Parse("M4 1C4 0.447715 4.44772 0 5 0C5.55228 0 6 0.447715 6 1V2H7V1C7 0.447715 7.44772 0 8 0C8.55228 0 9 0.447715 9 1V2H10V1C10 0.447715 10.4477 0 11 0C11.5523 0 12 0.447715 12 1V2.17071C12.8524 2.47199 13.528 3.14759 13.8293 4H15C15.5523 4 16 4.44772 16 5C16 5.55228 15.5523 6 15 6H14V7H15C15.5523 7 16 7.44772 16 8C16 8.55228 15.5523 9 15 9H14V10H15C15.5523 10 16 10.4477 16 11C16 11.5523 15.5523 12 15 12H13.8293C13.528 12.8524 12.8524 13.528 12 13.8293V15C12 15.5523 11.5523 16 11 16C10.4477 16 10 15.5523 10 15V14H9V15C9 15.5523 8.55228 16 8 16C7.44772 16 7 15.5523 7 15V14H6V15C6 15.5523 5.55228 16 5 16C4.44772 16 4 15.5523 4 15V13.8293C3.14759 13.528 2.47199 12.8524 2.17071 12H1C0.447715 12 0 11.5523 0 11C0 10.4477 0.447715 10 1 10H2V9H1C0.447715 9 0 8.55228 0 8C0 7.44772 0.447715 7 1 7H2V6H1C0.447715 6 0 5.55228 0 5C0 4.44772 0.447715 4 1 4H2.17071C2.47199 3.14759 3.14759 2.47199 4 2.17071V1ZM12 8V5C12 4.44772 11.5523 4 11 4H8H5C4.44772 4 4 4.44772 4 5V8V11C4 11.5523 4.44772 12 5 12H8H11C11.5523 12 12 11.5523 12 11V8Z");
        public static readonly Geometry? RamIcon                = Geometry.Parse("M3 0C1.34315 0 0 1.34315 0 3V5C0 6.65685 1.34315 8 3 8V11C3 11.5523 3.44772 12 4 12C4.55228 12 5 11.5523 5 11V8H7V11C7 11.5523 7.44772 12 8 12C8.55228 12 9 11.5523 9 11V8H11V11C11 11.5523 11.4477 12 12 12C12.5523 12 13 11.5523 13 11V8C14.6569 8 16 6.65685 16 5V3C16 1.34315 14.6569 0 13 0H3ZM2 3C2 2.44772 2.44772 2 3 2H13C13.5523 2 14 2.44772 14 3V5C14 5.55228 13.5523 6 13 6H3C2.44772 6 2 5.55228 2 5V3Z");
        
        #region DiskIcon
        public static readonly Geometry? DiskNormalIcon         = Geometry.Parse("M16 8C16 10.079 15.207 11.9727 13.9069 13.3953C13.8514 13.2206 13.7708 13.0507 13.6641 12.8906L11.6641 9.8906C11.4366 9.54931 11.1223 9.30051 10.77 9.15395C10.9182 8.79873 11 8.40892 11 8C11 6.34315 9.65685 5 8 5C6.34315 5 5 6.34315 5 8C5 9.65679 6.34303 10.9999 7.99979 11C7.99944 11.3818 8.10822 11.7679 8.3359 12.1094L10.3359 15.1094C10.4431 15.2702 10.5696 15.4105 10.7102 15.5292C9.86388 15.8339 8.95133 16 8 16C3.58172 16 0 12.4183 0 8C0 3.58172 3.58172 0 8 0C12.4183 0 16 3.58172 16 8ZM10.8321 10.4453C10.5257 9.98577 9.90483 9.8616 9.4453 10.1679C8.98577 10.4743 8.8616 11.0952 9.16795 11.5547L11.168 14.5547C11.4743 15.0142 12.0952 15.1384 12.5547 14.832C13.0142 14.5257 13.1384 13.9048 12.8321 13.4453L10.8321 10.4453Z");
        public static readonly Geometry? DiskReadIcon           = Geometry.Parse("M7.62152 5.02365C7.74548 5.00804 7.8718 5 8 5C9.65685 5 11 6.34315 11 8C11 8.40892 10.9182 8.79873 10.77 9.15395C11.1223 9.30051 11.4366 9.54931 11.6641 9.8906L13.6641 12.8906C13.7708 13.0507 13.8514 13.2206 13.9069 13.3953C15.207 11.9727 16 10.079 16 8C16 3.58172 12.4183 0 8 0C7.85702 0 7.71491 0.00375113 7.57377 0.0111604C8.07454 0.746319 8.33032 1.62238 8.33032 2.55C8.33032 3.42738 8.08432 4.29055 7.62152 5.02365ZM0.513765 10.8268C1.65593 13.8501 4.57689 16 8 16C8.95133 16 9.86388 15.8339 10.7102 15.5292C10.5696 15.4105 10.4431 15.2702 10.3359 15.1094L8.3359 12.1094C8.10822 11.7679 7.99944 11.3818 7.99979 11C7.74966 11 7.50668 10.9694 7.27439 10.9117C7.04374 10.9738 6.81706 11 6.61215 11H6.0975C5.6628 11 5.186 10.8917 4.74514 10.6095C4.39599 10.386 4.14022 10.099 3.95891 9.80364C3.86247 9.97062 3.7427 10.131 3.59691 10.2798C3.09892 10.7878 2.45302 11 1.85775 11H1.42888C1.12823 11 0.814665 10.9459 0.513765 10.8268ZM1 8.5625L1 0.4375C1 0.1625 1.1593 0 1.42888 0H3.36495C4.11456 0 4.75381 0.173183 5.24475 0.487118C5.93351 0.927557 6.33032 1.64504 6.33032 2.55C6.33032 3.2079 6.09477 3.82154 5.6898 4.27398C5.52539 4.45766 5.33305 4.61478 5.11721 4.7375L5.7473 6.0187L6.943 8.45C7.09004 8.75 6.943 9 6.61215 9H6.0975C5.88919 9 5.72989 8.9 5.63187 8.7L5.04204 7.49664L3.86734 5.1H2.28663V8.5625C2.28663 8.825 2.11508 9 1.85775 9H1.42888C1.17155 9 1 8.825 1 8.5625ZM3.73999 1.22733C3.62109 1.20933 3.49583 1.2 3.36495 1.2H2.28663L2.28663 2.40023V3.8875H3.36495C3.5435 3.8875 3.71161 3.87013 3.86734 3.83715C4.01952 3.80492 4.15988 3.75778 4.28663 3.69737C4.35067 3.66685 4.41123 3.63294 4.46809 3.59586C4.66668 3.46633 4.82006 3.29805 4.91837 3.1C5.00003 2.93549 5.04369 2.75044 5.04369 2.55C5.04369 2.03107 4.76012 1.61734 4.28663 1.39067C4.12524 1.31341 3.94179 1.25788 3.73999 1.22733ZM9.4453 10.1679C9.90483 9.8616 10.5257 9.98577 10.8321 10.4453L12.8321 13.4453C13.1384 13.9048 13.0142 14.5257 12.5547 14.832C12.0952 15.1384 11.4743 15.0142 11.168 14.5547L9.16795 11.5547C8.8616 11.0952 8.98577 10.4743 9.4453 10.1679Z");
        public static readonly Geometry? DiskWriteIcon          = Geometry.Parse("M1.47191 0C1.20974 0 1 0.198621 1 0.446897L1.11241 3.92799L1.26217 8.56552C1.26217 8.83862 1.44569 9 1.74719 9H1.95693C1.98315 9 2.0089 8.99932 2.03419 8.99796C2.37832 8.97949 2.63654 8.83534 2.80079 8.56552C2.81762 8.53787 2.83347 8.50891 2.84831 8.47862L4.5 5.35034L5.2568 6.78371L6.15169 8.47862C6.3221 8.82621 6.6236 9 7.04307 9H7.25281C7.55431 9 7.73783 8.83862 7.73783 8.56552L7.85285 5.00354L8 0.446897C8 0.431379 7.99918 0.416056 7.99758 0.400963C7.97962 0.231581 7.86332 0.0912089 7.70294 0.0312516C7.67712 0.0215981 7.65016 0.0140284 7.62228 0.00875856C7.62227 0.00875863 7.62227 0.0087587 7.62227 0.00875877C7.59187 0.00301234 7.56038 0 7.52809 0H7.0824C6.96914 0 6.87056 0.032434 6.79405 0.0902958C6.79308 0.0910327 6.79211 0.0917734 6.79114 0.0925185C6.69955 0.163089 6.6402 0.270525 6.6261 0.402528C6.62497 0.413026 6.62414 0.423679 6.6236 0.434483L6.4459 5.4334L6.40075 6.70345L5.93587 5.82298L5.62955 5.24281L5.18165 4.39448C5.17018 4.36966 5.15748 4.34638 5.14358 4.32466C5.04626 4.17259 4.88998 4.09655 4.68352 4.09655H4.31648C4.08052 4.09655 3.91011 4.19586 3.81835 4.39448L3.37047 5.24276L2.59925 6.70345L2.44053 2.24739L2.3764 0.446897C2.3633 0.173793 2.17978 0 1.9176 0H1.47191ZM0.4539 10.6629C1.55108 13.7721 4.51536 16 8 16C8.95133 16 9.86388 15.8339 10.7102 15.5292C10.5696 15.4105 10.4431 15.2702 10.3359 15.1094L8.3359 12.1094C8.10822 11.7679 7.99944 11.3818 7.99979 11C7.7668 11 7.54002 10.9734 7.3223 10.9231L7.25281 10.9319H6.37164C6.08118 10.8711 5.78455 10.7652 5.49697 10.5976C5.06629 10.3467 4.73665 10.0039 4.5 9.62029C4.26334 10.0039 3.93371 10.3467 3.50303 10.5976C3.21545 10.7652 2.91882 10.8711 2.62836 10.9319H1.52868L0.744103 10.8059C0.647046 10.7659 0.54993 10.7186 0.4539 10.6629ZM9.83381 5.62555C10.5432 6.17423 11 7.03377 11 8C11 8.40892 10.9182 8.79873 10.77 9.15395C11.1223 9.30051 11.4366 9.54931 11.6641 9.8906L13.6641 12.8906C13.7708 13.0507 13.8514 13.2206 13.9069 13.3953C15.207 11.9727 16 10.079 16 8C16 4.26962 13.4468 1.13559 9.99251 0.250111C9.99747 0.314705 10 0.380317 10 0.446895V0.47918L9.83381 5.62555ZM9.4453 10.1679C9.90483 9.8616 10.5257 9.98577 10.8321 10.4453L12.8321 13.4453C13.1384 13.9048 13.0142 14.5257 12.5547 14.832C12.0952 15.1384 11.4743 15.0142 11.168 14.5547L9.16795 11.5547C8.8616 11.0952 8.98577 10.4743 9.4453 10.1679Z");
        #endregion

        #region NetworkIcon
        public static readonly Geometry? NetworkNormalIcon      = Geometry.Parse("M14 1C14 0.447715 13.5523 0 13 0C12.4477 0 12 0.447715 12 1L12 15C12 15.5523 12.4477 16 13 16C13.5523 16 14 15.5523 14 15L14 1ZM2 13C2 12.4477 1.55228 12 1 12C0.447715 12 0 12.4477 0 13V15C0 15.5523 0.447715 16 1 16C1.55228 16 2 15.5523 2 15L2 13ZM9.00186 4C9.55414 4.00103 10.001 4.44957 10 5.00186L9.98144 15.0018C9.98041 15.5541 9.53187 16.001 8.97958 16C8.4273 15.9989 7.98042 15.5504 7.98144 14.9981L8 4.99814C8.00103 4.44586 8.44957 3.99898 9.00186 4ZM6 9C6 8.44772 5.55228 8 5 8C4.44772 8 4 8.44772 4 9L4 15C4 15.5523 4.44772 16 5 16C5.55229 16 6 15.5523 6 15L6 9Z");
        public static readonly Geometry? NetworkSentIcon        = Geometry.Parse("M5.25259 0.341495C5.0627 0.12448 4.78838 0 4.50001 0C4.21165 0 3.93733 0.12448 3.74744 0.341495L0.247437 4.3415C-0.116245 4.75713 -0.0741276 5.38889 0.341509 5.75258C0.757146 6.11626 1.38891 6.07414 1.75259 5.6585L3.50001 3.66145V11C3.50001 11.5523 3.94773 12 4.50001 12C5.0523 12 5.50001 11.5523 5.50001 11V3.66145L7.24744 5.6585C7.61112 6.07414 8.24288 6.11626 8.65852 5.75258C9.07415 5.38889 9.11627 4.75713 8.75259 4.3415L5.25259 0.341495ZM4 15V12.937C4.15981 12.9781 4.32736 13 4.50001 13C5.09735 13 5.63353 12.7381 6 12.3229V15C6 15.5523 5.55229 16 5 16C4.44772 16 4 15.5523 4 15ZM7.98144 14.9981L7.99629 7.00002C8.46518 7.00108 8.93641 6.83819 9.31702 6.50515C9.76447 6.11364 9.99547 5.56678 9.99997 5.01561L9.98144 15.0018C9.98041 15.5541 9.53187 16.001 8.97958 16C8.4273 15.9989 7.98042 15.5504 7.98144 14.9981ZM14 1C14 0.447715 13.5523 0 13 0C12.4477 0 12 0.447715 12 1L12 15C12 15.5523 12.4477 16 13 16C13.5523 16 14 15.5523 14 15L14 1ZM2 13C2 12.4477 1.55228 12 1 12C0.447715 12 2.77664e-10 12.4477 2.77664e-10 13V15C2.77664e-10 15.5523 0.447715 16 1 16C1.55228 16 2 15.5523 2 15L2 13Z");
        public static readonly Geometry? NetworkReceivedIcon    = Geometry.Parse("M4.50001 13C4.32968 13 4.1618 12.9783 4 12.9365V15C4 15.5523 4.44772 16 5 16C5.55229 16 6 15.5523 6 15V12.3229C5.62042 12.7533 5.07413 13 4.50001 13ZM7.98144 14.9981L7.99063 10.0479L9.50516 8.31701C9.80717 7.97186 9.96926 7.5522 9.99605 7.12734L9.98144 15.0018C9.98041 15.5541 9.53187 16.001 8.97958 16C8.4273 15.9989 7.98042 15.5504 7.98144 14.9981ZM10 5.00186L9.99651 6.88169C9.96592 6.36583 9.73665 5.86202 9.31702 5.49485C8.93741 5.16269 8.46766 4.99978 8 4.99997L8 4.99814C8.00103 4.44586 8.44957 3.99898 9.00186 4C9.55414 4.00103 10.001 4.44957 10 5.00186ZM14 1C14 0.447715 13.5523 0 13 0C12.4477 0 12 0.447715 12 1L12 15C12 15.5523 12.4477 16 13 16C13.5523 16 14 15.5523 14 15L14 1ZM2 13C2 12.4477 1.55228 12 1 12C0.447715 12 0 12.4477 0 13V15C0 15.5523 0.447715 16 1 16C1.55228 16 2 15.5523 2 15L2 13ZM3.74744 11.6585C3.93733 11.8755 4.21165 12 4.50001 12C4.78838 12 5.0627 11.8755 5.25259 11.6585L8.75259 7.6585C9.11627 7.24287 9.07415 6.61111 8.65852 6.24742C8.24288 5.88374 7.61112 5.92586 7.24744 6.3415L5.50001 8.33855V1C5.50001 0.447715 5.0523 0 4.50001 0C3.94773 0 3.50001 0.447715 3.50001 1V8.33855L1.75259 6.3415C1.38891 5.92586 0.757146 5.88374 0.34151 6.24742C-0.0741272 6.61111 -0.116244 7.24287 0.247437 7.6585L3.74744 11.6585Z");
        #endregion

        public static readonly Geometry? SoundIcon              = Geometry.Parse("M5 1C5 0.447715 4.55228 0 4 0C3.44772 0 3 0.447715 3 1L3 13C3 13.5523 3.44772 14 4 14C4.55229 14 5 13.5523 5 13L5 1ZM10 2C10.5523 2 11 2.44772 11 3V11C11 11.5523 10.5523 12 10 12C9.44771 12 9 11.5523 9 11V3C9 2.44772 9.44771 2 10 2ZM1 4C1.55228 4 2 4.44772 2 5L2 9C2 9.55229 1.55228 10 1 10C0.447715 10 0 9.55229 0 9V5C0 4.44772 0.447715 4 1 4ZM14 5C14 4.44772 13.5523 4 13 4C12.4477 4 12 4.44772 12 5V9C12 9.55229 12.4477 10 13 10C13.5523 10 14 9.55229 14 9V5ZM7 3C7.55228 3 8 3.44772 8 4V10C8 10.5523 7.55229 11 7 11C6.44772 11 6 10.5523 6 10L6 4C6 3.44772 6.44772 3 7 3Z");

        #region PowerIcon
        public static readonly Geometry? PowerIconEmpty         = Geometry.Parse("M3 0C1.34315 0 0 1.34315 0 3V11C0 12.6569 1.34315 14 3 14H11C12.6569 14 14 12.6569 14 11V10H15C15.5523 10 16 9.55228 16 9V5C16 4.44772 15.5523 4 15 4H14V3C14 1.34315 12.6569 0 11 0H3ZM2 3C2 2.44772 2.44772 2 3 2H11C11.5523 2 12 2.44772 12 3V11C12 11.5523 11.5523 12 11 12H3C2.44772 12 2 11.5523 2 11V3Z");
        public static readonly Geometry? PowerIconCharging      = Geometry.Parse("M7.77069 0.549417C7.80586 0.317045 7.67572 0.0923773 7.4651 0.0218469C7.25448 -0.0486834 7.02724 0.0563044 6.93147 0.268382L3.04399 8.87753C2.97769 9.02437 2.98681 9.19724 3.06814 9.33523C3.14946 9.47321 3.29159 9.55696 3.44445 9.55696H7.12108L6.22931 15.4506C6.19415 15.683 6.32428 15.9076 6.5349 15.9782C6.74552 16.0487 6.97276 15.9437 7.06853 15.7316L10.956 7.12247C11.0223 6.97563 11.0132 6.80276 10.9319 6.66477C10.8505 6.52679 10.7084 6.44304 10.5556 6.44304H6.87892L7.77069 0.549417ZM3 1H5.56436L4.64608 3H3C2.44772 3 2 3.44772 2 4V9.08921V9.08943V12C2 12.5523 2.44772 13 3 13H5.67466L5.36695 15H3C1.34315 15 0 13.6569 0 12V4C0 2.34315 1.34315 1 3 1ZM12 4V6.90967V6.9117V12C12 12.5523 11.5523 13 11 13H9.35392L8.43564 15H11C12.6569 15 14 13.6569 14 12V11H15C15.5523 11 16 10.5523 16 10V6C16 5.44772 15.5523 5 15 5H14V4C14 2.34315 12.6569 1 11 1H8.63305L8.32534 3H11C11.5523 3 12 3.44772 12 4Z");
        public static readonly Geometry? PowerIconHigh          = Geometry.Parse("M0 3C0 1.34315 1.34315 0 3 0H11C12.6569 0 14 1.34315 14 3V4H15C15.5523 4 16 4.44772 16 5V9C16 9.55228 15.5523 10 15 10H14V11C14 12.6569 12.6569 14 11 14H3C1.34315 14 0 12.6569 0 11V3ZM3 2C2.44772 2 2 2.44772 2 3V11C2 11.5523 2.44772 12 3 12H11C11.5523 12 12 11.5523 12 11V3C12 2.44772 11.5523 2 11 2H3ZM4 5C4 4.44772 4.44772 4 5 4H9C9.55228 4 10 4.44772 10 5V9C10 9.55228 9.55228 10 9 10H5C4.44772 10 4 9.55228 4 9V5Z");
        public static readonly Geometry? PowerIconLow           = Geometry.Parse("M0 3C0 1.34315 1.34315 0 3 0H11C12.6569 0 14 1.34315 14 3V4H15C15.5523 4 16 4.44772 16 5V9C16 9.55228 15.5523 10 15 10H14V11C14 12.6569 12.6569 14 11 14H3C1.34315 14 0 12.6569 0 11V3ZM3 2C2.44772 2 2 2.44772 2 3V11C2 11.5523 2.44772 12 3 12H11C11.5523 12 12 11.5523 12 11V3C12 2.44772 11.5523 2 11 2H3ZM4 5C4 4.44772 4.44772 4 5 4H7C7.55228 4 8 4.44772 8 5V9C8 9.55228 7.55228 10 7 10H5C4.44772 10 4 9.55228 4 9V5Z");
        public static readonly Geometry? PowerIconCritical      = Geometry.Parse("M0 3C0 1.34315 1.34315 0 3 0H11C12.6569 0 14 1.34315 14 3V4H15C15.5523 4 16 4.44772 16 5V9C16 9.55228 15.5523 10 15 10H14V11C14 12.6569 12.6569 14 11 14H3C1.34315 14 0 12.6569 0 11V3ZM3 2C2.44772 2 2 2.44772 2 3V11C2 11.5523 2.44772 12 3 12H11C11.5523 12 12 11.5523 12 11V3C12 2.44772 11.5523 2 11 2H3ZM4 5C4 4.44772 4.44772 4 5 4C5.55228 4 6 4.44772 6 5V9C6 9.55228 5.55228 10 5 10C4.44772 10 4 9.55228 4 9V5Z");
        #endregion
    }
}
