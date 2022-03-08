-- 隠せぬ焦り カーディア

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.PlacedOnRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation()
end

function Trigger()
	return obj.IsApplicable()
end

function Condition()
	return obj.IsSameZone() and obj.Exists({q.Location, l.PlayerVC, q.NameContains, obj.LoadName("Astesice")})
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 10000, p.UntilEndOfTurn)
end