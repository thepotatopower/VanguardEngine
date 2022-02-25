-- ディアブロスチャージャー ダヴァン

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnStand)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCondition("Condition")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.Applicable})
end

function Condition()
	return obj.IsSameZone()
end

function Activation()
	obj.AddCardValue({q.Other, o.ThisFieldID}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end