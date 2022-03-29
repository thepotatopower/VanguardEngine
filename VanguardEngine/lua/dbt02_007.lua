-- 唱導の天弓 レフェルソス

function RegisterAbilities()
	-- cont
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.Cont)
	ability1.SetLocation(l.RC)
	ability1.SetActivation("Cont")
	-- on stand
	local ability2 = NewAbility(GetID())
	ability2.SetDescription(2)
	ability2.SetTiming(a.OnStand)
	ability2.SetLocation(l.BackRowRC)
	ability2.SetProperty(p.OncePerTurn)
	ability2.SetTrigger("Trigger")
	ability2.SetCondition("Condition")
	ability2.SetActivation("Activation")
end

function Cont()
	if obj.PersonaRode() then
		obj.AddCardValue({q.Other, o.This}, cs.BonusSkills, s.Boost, p.Continuous)
	end
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Applicable, q.Other, o.NotThisFieldID})
end

function Condition()
	return obj.Exists({q.Location, l.PlayerUnits, q.Other, o.Resting, q.Other, o.ThisFieldID})
end

function Activation()
	obj.Stand({q.Other, o.ThisFieldID})
end