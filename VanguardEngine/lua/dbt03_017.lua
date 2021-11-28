-- 極光戦姫 エグゼキュート・レモナン

function RegisterAbilities()
	-- on attack
	local ability1 = NewAbility(GetID())
	ability1.SetDescription(1)
	ability1.SetTiming(a.OnAttack)
	ability1.SetLocation(l.PlayerRC)
	ability1.SetTrigger("Trigger")
	ability1.SetCost("Cost")
	ability1.SetCanFullyResolve("CanFullyResolve")
	ability1.SetActivation("Activation")
end

function Trigger()
	return obj.IsAttackingUnit()
end

function CanFullyResolve()
	return obj.HasPrison() and obj.IsSameZone()
end

function Cost(check)
	if check then return obj.CanCB(1) and obj.CanSB(3) end
	obj.CounterBlast(1)
	obj.SoulBlast(3)
end

function Activation()
	if obj.HasPrison() and obj.IsSameZone() then
		obj.ChooseImprison({q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 3})
		local count = obj.GetNumberOf({q.Location, l.PlayerPrisoners})
		if count >= 3 then
			obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusPower, 10000, p.UntilEndOfBattle)
		end
		if count >= 5 then
			obj.AddCardValue({q.Location, l.PlayerRC, q.Other, o.This}, cs.BonusCritical, 1, p.UntilEndOfBattle)
		end
	end
end