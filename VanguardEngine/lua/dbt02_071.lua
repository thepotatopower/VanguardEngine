-- Diabolos Attacker, Arwing

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.SB, 1
	end
end

function CheckCondition(n)
	if n == 1 then
		if (obj.IsRearguard() or obj.IsVanguard()) and obj.IsAttackingUnit() and obj.TargetIsEnemyVanguard() then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.InFinalRush() then
			obj.AddBattleOnlyPower(1, 15000)
		else
			obj.AddBattleOnlyPower(1, 5000)
		end
	end
	return 0
end