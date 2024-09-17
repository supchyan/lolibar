﻿using System.Windows.Media;

namespace LolibarApp.Source.Tools
{
    public partial class LolibarDefaults
    {
        public static readonly Geometry? CurProcIcon         = Geometry.Parse("M6 0C6.37877 0 6.72504 0.214002 6.89443 0.552786L10 6.76393L11.1056 4.55279C11.275 4.214 11.6212 4 12 4H15C15.5523 4 16 4.44772 16 5C16 5.55228 15.5523 6 15 6H12.618L10.8944 9.44721C10.725 9.786 10.3788 10 10 10C9.62123 10 9.27497 9.786 9.10557 9.44721L6 3.23607L4.89443 5.44721C4.72504 5.786 4.37877 6 4 6H1C0.447715 6 0 5.55228 0 5C0 4.44772 0.447715 4 1 4H3.38197L5.10557 0.552786C5.27496 0.214002 5.62123 0 6 0Z");
        public static readonly Geometry? CpuIcon             = Geometry.Parse("M4 1C4 0.447715 4.44772 0 5 0C5.55228 0 6 0.447715 6 1V2H7V1C7 0.447715 7.44772 0 8 0C8.55228 0 9 0.447715 9 1V2H10V1C10 0.447715 10.4477 0 11 0C11.5523 0 12 0.447715 12 1V2.17071C12.8524 2.47199 13.528 3.14759 13.8293 4H15C15.5523 4 16 4.44772 16 5C16 5.55228 15.5523 6 15 6H14V7H15C15.5523 7 16 7.44772 16 8C16 8.55228 15.5523 9 15 9H14V10H15C15.5523 10 16 10.4477 16 11C16 11.5523 15.5523 12 15 12H13.8293C13.528 12.8524 12.8524 13.528 12 13.8293V15C12 15.5523 11.5523 16 11 16C10.4477 16 10 15.5523 10 15V14H9V15C9 15.5523 8.55228 16 8 16C7.44772 16 7 15.5523 7 15V14H6V15C6 15.5523 5.55228 16 5 16C4.44772 16 4 15.5523 4 15V13.8293C3.14759 13.528 2.47199 12.8524 2.17071 12H1C0.447715 12 0 11.5523 0 11C0 10.4477 0.447715 10 1 10H2V9H1C0.447715 9 0 8.55228 0 8C0 7.44772 0.447715 7 1 7H2V6H1C0.447715 6 0 5.55228 0 5C0 4.44772 0.447715 4 1 4H2.17071C2.47199 3.14759 3.14759 2.47199 4 2.17071V1ZM12 8V5C12 4.44772 11.5523 4 11 4H8H5C4.44772 4 4 4.44772 4 5V8V11C4 11.5523 4.44772 12 5 12H8H11C11.5523 12 12 11.5523 12 11V8Z");
        public static readonly Geometry? RamIcon             = Geometry.Parse("M3 0C1.34315 0 0 1.34315 0 3V5C0 6.65685 1.34315 8 3 8V11C3 11.5523 3.44772 12 4 12C4.55228 12 5 11.5523 5 11V8H7V11C7 11.5523 7.44772 12 8 12C8.55228 12 9 11.5523 9 11V8H11V11C11 11.5523 11.4477 12 12 12C12.5523 12 13 11.5523 13 11V8C14.6569 8 16 6.65685 16 5V3C16 1.34315 14.6569 0 13 0H3ZM2 3C2 2.44772 2.44772 2 3 2H13C13.5523 2 14 2.44772 14 3V5C14 5.55228 13.5523 6 13 6H3C2.44772 6 2 5.55228 2 5V3Z");
        public static readonly Geometry? GpuIcon             = Geometry.Parse("M5 0C4.44772 0 4 0.447715 4 1V2.17071C3.14759 2.47199 2.47199 3.14759 2.17071 4H1C0.447715 4 0 4.44772 0 5C0 5.55228 0.447715 6 1 6H2V7H1C0.447715 7 0 7.44772 0 8C0 8.55228 0.447715 9 1 9H2V10H1C0.447715 10 0 10.4477 0 11C0 11.5523 0.447715 12 1 12H2.17071C2.47199 12.8524 3.14759 13.528 4 13.8293V15C4 15.5523 4.44772 16 5 16C5.55228 16 6 15.5523 6 15V14H7V15C7 15.5523 7.44772 16 8 16C8.55228 16 9 15.5523 9 15V14H10V15C10 15.5523 10.4477 16 11 16C11.5523 16 12 15.5523 12 15V13.8293C12.8524 13.528 13.528 12.8524 13.8293 12H15C15.5523 12 16 11.5523 16 11C16 10.4477 15.5523 10 15 10H14V9H15C15.5523 9 16 8.55228 16 8C16 7.44772 15.5523 7 15 7H14V6H15C15.5523 6 16 5.55228 16 5C16 4.44772 15.5523 4 15 4H13.8293C13.528 3.14759 12.8524 2.47199 12 2.17071V1C12 0.447715 11.5523 0 11 0C10.4477 0 10 0.447715 10 1V2H9V1C9 0.447715 8.55228 0 8 0C7.44772 0 7 0.447715 7 1V2H6V1C6 0.447715 5.55228 0 5 0ZM12 5V8V11C12 11.5523 11.5523 12 11 12H8H5C4.44772 12 4 11.5523 4 11V8V5C4 4.44772 4.44772 4 5 4H8H11C11.5523 4 12 4.44772 12 5ZM6.89443 5.55279C6.64744 5.05881 6.04676 4.85858 5.55279 5.10557C5.05881 5.35256 4.85858 5.95324 5.10557 6.44721L7.10557 10.4472C7.27496 10.786 7.62123 11 8 11C8.37877 11 8.72503 10.786 8.89443 10.4472L10.8944 6.44721C11.1414 5.95324 10.9412 5.35256 10.4472 5.10557C9.95324 4.85858 9.35256 5.05881 9.10557 5.55279L8 7.76393L6.89443 5.55279Z");
        public static readonly Geometry? DiskIcon            = Geometry.Parse("M16 8C16 12.4183 12.4183 16 8 16C3.58172 16 0 12.4183 0 8C0 3.58172 3.58172 0 8 0C12.4183 0 16 3.58172 16 8ZM11 8C11 9.65685 9.65685 11 8 11C6.34315 11 5 9.65685 5 8C5 6.34315 6.34315 5 8 5C9.65685 5 11 6.34315 11 8Z");
        public static readonly Geometry? NetworkIcon         = Geometry.Parse("M6 3C6 4.30622 5.16519 5.41746 4 5.82929V8.17071C4.3554 8.29632 4.68006 8.487 4.95967 8.72844L7.04762 7.53532C7.01633 7.36162 7 7.18272 7 7C7 5.34315 8.34315 4 10 4C11.6569 4 13 5.34315 13 7C13 8.65685 11.6569 10 10 10C9.25095 10 8.56602 9.72548 8.04033 9.27156L5.95238 10.4647C5.98367 10.6384 6 10.8173 6 11C6 12.6569 4.65685 14 3 14C1.34315 14 0 12.6569 0 11C0 9.69378 0.834808 8.58254 2 8.17071V5.82929C0.834808 5.41746 0 4.30622 0 3C0 1.34315 1.34315 0 3 0C4.65685 0 6 1.34315 6 3ZM4 3C4 3.55228 3.55228 4 3 4C2.44772 4 2 3.55228 2 3C2 2.44772 2.44772 2 3 2C3.55228 2 4 2.44772 4 3ZM10 8C10.5523 8 11 7.55228 11 7C11 6.44772 10.5523 6 10 6C9.44772 6 9 6.44772 9 7C9 7.55228 9.44772 8 10 8ZM3 12C3.55228 12 4 11.5523 4 11C4 10.4477 3.55228 10 3 10C2.44772 10 2 10.4477 2 11C2 11.5523 2.44772 12 3 12Z");

        public static readonly Geometry? SoundIcon           = Geometry.Parse("M5 1C5 0.447715 4.55228 0 4 0C3.44772 0 3 0.447715 3 1L3 13C3 13.5523 3.44772 14 4 14C4.55229 14 5 13.5523 5 13L5 1ZM10 2C10.5523 2 11 2.44772 11 3V11C11 11.5523 10.5523 12 10 12C9.44771 12 9 11.5523 9 11V3C9 2.44772 9.44771 2 10 2ZM1 4C1.55228 4 2 4.44772 2 5L2 9C2 9.55229 1.55228 10 1 10C0.447715 10 0 9.55229 0 9V5C0 4.44772 0.447715 4 1 4ZM14 5C14 4.44772 13.5523 4 13 4C12.4477 4 12 4.44772 12 5V9C12 9.55229 12.4477 10 13 10C13.5523 10 14 9.55229 14 9V5ZM7 3C7.55228 3 8 3.44772 8 4V10C8 10.5523 7.55229 11 7 11C6.44772 11 6 10.5523 6 10L6 4C6 3.44772 6.44772 3 7 3Z");

        #region PowerIcon
        public static          Geometry? PowerIcon           {  get; private set; }
        public static readonly Geometry? PowerIconEmpty      = Geometry.Parse("M3 0C1.34315 0 0 1.34315 0 3V11C0 12.6569 1.34315 14 3 14H11C12.6569 14 14 12.6569 14 11V10H15C15.5523 10 16 9.55228 16 9V5C16 4.44772 15.5523 4 15 4H14V3C14 1.34315 12.6569 0 11 0H3ZM2 3C2 2.44772 2.44772 2 3 2H11C11.5523 2 12 2.44772 12 3V11C12 11.5523 11.5523 12 11 12H3C2.44772 12 2 11.5523 2 11V3Z");
        public static readonly Geometry? PowerIconCharging   = Geometry.Parse("M7.77069 0.549417C7.80586 0.317045 7.67572 0.0923773 7.4651 0.0218469C7.25448 -0.0486834 7.02724 0.0563044 6.93147 0.268382L3.04399 8.87753C2.97769 9.02437 2.98681 9.19724 3.06814 9.33523C3.14946 9.47321 3.29159 9.55696 3.44445 9.55696H7.12108L6.22931 15.4506C6.19415 15.683 6.32428 15.9076 6.5349 15.9782C6.74552 16.0487 6.97276 15.9437 7.06853 15.7316L10.956 7.12247C11.0223 6.97563 11.0132 6.80276 10.9319 6.66477C10.8505 6.52679 10.7084 6.44304 10.5556 6.44304H6.87892L7.77069 0.549417ZM3 1H5.56436L4.64608 3H3C2.44772 3 2 3.44772 2 4V9.08921V9.08943V12C2 12.5523 2.44772 13 3 13H5.67466L5.36695 15H3C1.34315 15 0 13.6569 0 12V4C0 2.34315 1.34315 1 3 1ZM12 4V6.90967V6.9117V12C12 12.5523 11.5523 13 11 13H9.35392L8.43564 15H11C12.6569 15 14 13.6569 14 12V11H15C15.5523 11 16 10.5523 16 10V6C16 5.44772 15.5523 5 15 5H14V4C14 2.34315 12.6569 1 11 1H8.63305L8.32534 3H11C11.5523 3 12 3.44772 12 4Z");
        public static readonly Geometry? PowerIconHigh       = Geometry.Parse("M0 3C0 1.34315 1.34315 0 3 0H11C12.6569 0 14 1.34315 14 3V4H15C15.5523 4 16 4.44772 16 5V9C16 9.55228 15.5523 10 15 10H14V11C14 12.6569 12.6569 14 11 14H3C1.34315 14 0 12.6569 0 11V3ZM3 2C2.44772 2 2 2.44772 2 3V11C2 11.5523 2.44772 12 3 12H11C11.5523 12 12 11.5523 12 11V3C12 2.44772 11.5523 2 11 2H3ZM4 5C4 4.44772 4.44772 4 5 4H9C9.55228 4 10 4.44772 10 5V9C10 9.55228 9.55228 10 9 10H5C4.44772 10 4 9.55228 4 9V5Z");
        public static readonly Geometry? PowerIconLow        = Geometry.Parse("M0 3C0 1.34315 1.34315 0 3 0H11C12.6569 0 14 1.34315 14 3V4H15C15.5523 4 16 4.44772 16 5V9C16 9.55228 15.5523 10 15 10H14V11C14 12.6569 12.6569 14 11 14H3C1.34315 14 0 12.6569 0 11V3ZM3 2C2.44772 2 2 2.44772 2 3V11C2 11.5523 2.44772 12 3 12H11C11.5523 12 12 11.5523 12 11V3C12 2.44772 11.5523 2 11 2H3ZM4 5C4 4.44772 4.44772 4 5 4H7C7.55228 4 8 4.44772 8 5V9C8 9.55228 7.55228 10 7 10H5C4.44772 10 4 9.55228 4 9V5Z");
        public static readonly Geometry? PowerIconCritical   = Geometry.Parse("M0 3C0 1.34315 1.34315 0 3 0H11C12.6569 0 14 1.34315 14 3V4H15C15.5523 4 16 4.44772 16 5V9C16 9.55228 15.5523 10 15 10H14V11C14 12.6569 12.6569 14 11 14H3C1.34315 14 0 12.6569 0 11V3ZM3 2C2.44772 2 2 2.44772 2 3V11C2 11.5523 2.44772 12 3 12H11C11.5523 12 12 11.5523 12 11V3C12 2.44772 11.5523 2 11 2H3ZM4 5C4 4.44772 4.44772 4 5 4C5.55228 4 6 4.44772 6 5V9C6 9.55228 5.55228 10 5 10C4.44772 10 4 9.55228 4 9V5Z");
        #endregion
    }
}
