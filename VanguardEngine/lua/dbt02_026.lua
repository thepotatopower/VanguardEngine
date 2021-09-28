-- Strong Fortress Dragon, Jibrabrachio

function NumberOfAbilities()
	return 2
end

function NumberOfParams()
	return 4
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Other, o.This, q.Other, o.Standing
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	elseif n == 3 then
		return q.Location, l.EnemyRC, q.Count, 2
	elseif n == 4 then
		return q.Location, l.PlayerRC, q.Other, o.This
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnACT, p.HasPrompt, p.SB, 1, p.Rest, 1
	elseif n == 2 then
		return a.OnBattlePhase, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.VanguardIs("Heavy Artillery of Dust Storm, Eugene") then
			return true
		end
	elseif n == 2 then
		if obj.IsRearguard() and obj.IsPlayerTurn() and not obj.Exists(3) then
			return true
		end
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.CanRetire(2) then
			return true
		end
	elseif n == 2 then
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseRetire(2)
	elseif n == 2 then
		obj.Stand(4)
		obj.AddTempPower(4, 5000)
	end
	return 0
end