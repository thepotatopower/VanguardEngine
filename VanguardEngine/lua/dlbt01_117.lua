-- Vibrant Symphony

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 1
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerUnits, q.Other, o.Attacked, q.Count, 1
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
		if obj.PlayerHasState(ps.PlayerVanguardHitThisTurn) then
			return true
		end
	end
	return false
end

function Activate(n)
	if n == 1 then
		if obj.PlayerHasState(ps.PlayerVanguardHitThisTurn) then
			obj.ChooseAddBattleOnlyPower(1, 30000)
		end
	end
	return 0
end