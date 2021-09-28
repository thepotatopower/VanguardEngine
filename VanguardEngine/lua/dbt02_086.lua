-- Cardinal Fang, Estrett

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.EnemyRC, q.Other, o.CanChoose, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnEndPhase, p.HasPrompt, p.IsMandatory
	end
end

function CheckCondition(n)
	if n == 1 then
		if obj.IsRearguard() and obj.IsAbyssalDarkNight() then
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
		if obj.IsPlayerTurn() then
			obj.ChooseRetire(1)
			obj.EnemyChooseRetire(2)
		else
			obj.EnemyChooseRetire(2)
			obj.ChooseRetire(1)
		end
	end
	return 0
end