-- Cardinal Draco, Alviderd

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 3
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.Token, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.Cont, p.IsMandatory
	elseif n == 2 then
		return a.OnBattleEnds, p.HasPrompt, p.Retire, 2
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() then
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
	elseif n == 2 then
		if obj.Exists(3) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.IsPlayerTurn() and obj.IsDarkNight() then
			obj.SetAbilityPower(1, 2000)
		elseif obj.IsPlayerTurn() and obj.IsAbyssalDarkNight() then
			obj.SetAbilityPower(1, 5000)
		end
	elseif n == 2 then
		obj.ChooseRetire(3)
	end
	return 0
end
