-- 異能発現・重力加速

function RegisterAbilities()
	-- blitz order
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnBlitzOrder)
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function CanFullyResolve()
	return obj.SoulCount() >= 4
end

function Activation()
	if obj.SoulCount() >= 4 then
		obj.SoulCharge(1)
		obj.Select({q.Location, l.EnemyVC, q.Other, o.CanChoose, q.Other, o.Attacking, q.Count, 1})
		obj.AddCardValue({q.Location, l.Selected,}, cs.BonusPower, -5000, p.UntilEndOfBattle)
	end
end