-- Overserious President, Equinoa

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnAttack, p.HasPrompt, p.OncePerTurn
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsApplicable() and obj.Exists(1) then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsAttackingUnit() then
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
		obj.CounterCharge(1)
		obj.ChooseStand(2)
	elseif n == 2 then
		obj.AddBattleOnlyPower(3, 5000)
		obj.AddUntilEndOfBattleState(3, cs.SendToBottomAtEndOfBattle)
	end
	return 0
end