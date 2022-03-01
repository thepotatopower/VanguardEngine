-- 腐滅の簒竜

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnVC)
	ability1.SetLocation(l.Drop)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.Exists({q.Location, l.Applicable, q.Grade, 3, q.Other, o.GradeOrHigher})
end

function Condition()
	return obj.CanSuperiorCall({q.Other, o.ThisFieldID})
end

function Activation()
	obj.SuperiorCall({q.Other, o.ThisFieldID})
end