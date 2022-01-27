-- ブレインウォッシュ・スワラー

function RegisterAbilities()
	-- on place
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("OnPlaceTrigger")
	ability1.SetActivation("OnPlace")
	-- on soul charge
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnSoulCharge)
	ability2.SetLocation(l.RC)
	ability2.SetTrigger("OnSoulChargeTrigger")
	ability2.SetCondition("OnSoulChargeCondition")
	ability2.SetActivation("OnSoulCharge")
end

function OnPlaceTrigger()
	return obj.IsApplicable()
end

function OnPlace()
	obj.SoulCharge(1)
end

function OnSoulChargeTrigger()
	return obj.Exists({q.Location, l.Applicable, q.Other, o.Player})
end

function OnSoulChargeCondition()
	return obj.Exists({q.Location, l.PlayerVC, q.Grade, 3})
end

function OnSoulCharge()
	obj.AddCardValue({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end