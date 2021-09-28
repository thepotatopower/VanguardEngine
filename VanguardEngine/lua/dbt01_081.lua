-- Tartarus Beatscram

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerVC, q.Location, l.PlayerRC, q.Count, 1
	end
end

function ActivationRequirement(n)
	if n == 1 then
		return a.OnBlitzOrder, p.HasPrompt
	end
end

function CheckCondition(n)
	if n == 1 then
		return true
	end
	return false
end

function CanFullyResolve(n)
	if n == 1 then
		if obj.SoulCount() >= 5 then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.SoulCount() >= 5 then
			obj.ChooseAddBattleOnlyPower(1, 15000)
		end
	end
	return 0
end