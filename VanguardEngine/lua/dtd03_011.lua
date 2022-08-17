-- 破約の魔女 フェクティル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnDiscard)
	ability1.SetMovedFrom(l.Hand)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsApplicable() and obj.PlayerRidePhase()
end

function Condition()
	return obj.CanSuperiorCall({q.Other, o.ThisFieldID})
end

function Activation()
	obj.SuperiorCall({q.Other, o.ThisFieldID})
end