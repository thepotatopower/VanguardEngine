-- 轟然の天斬 ゲトゥーゼ

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
	return obj.SourceIsVanguardAbility() and obj.IsApplicable()
end

function Condition()
	return obj.Exists({q.Location, l.PlayerRC, q.Other, o.NotThis})
end

function Activation()
	obj.Select({q.Location, l.PlayerRC, q.Other, o.NotThis, q.Count, 1})
	obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfTurn)
end