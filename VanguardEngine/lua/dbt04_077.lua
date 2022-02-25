-- ディアブロスマドンナ リース

function RegisterAbilities()
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttackHits)
	ability1.SetLocation(l.RC)
	ability1.SetTrigger("Trigger")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit() and obj.Exists({q.Location, l.EnemyVC, q.Other, o.Attacked})
end

function Activation()
	obj.SoulCharge(2)
	if obj.InFinalRush() then
		obj.Select({q.Location, l.PlayerUnits, q.Count, 1})
		obj.AddCardValue({q.Location, l.Selected}, cs.BonusPower, 10000, p.UntilEndOfTurn)
	end
end