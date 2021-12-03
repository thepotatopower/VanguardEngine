-- 柩機の獣 キネティア

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.Exists({q.Location, l.PlayerVC, q.Other, o.Attacked})
end

function Cost(check)
	if check then return obj.CanRetire({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID}) end
	obj.Retire({q.Location, l.PlayerUnits, q.Other, o.ThisFieldID})
end

function Activation()
	obj.Select({q.Location, l.PlayerVC, q.Count, 1})
	if obj.IsDarkNight() then
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 5000, p.UntilEndOfBattle)
	else
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfBattle)
	end
end