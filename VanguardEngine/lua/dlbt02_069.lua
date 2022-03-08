-- アウェイティングスマイル マルエル

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetProperty(p.BlackWings)
	ability1.SetTiming(a.OnBattleEnds)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
	ability1.SetProperty(p.NotMandatory)
end

function Trigger()
	return obj.IsBooster()
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddToHand({q.Other, o.ThisFieldID})
end