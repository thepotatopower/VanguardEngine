-- Ghost Chase

function NumberOfAbilities()
	return 1
end

function NumberOfParams()
	return 2
end

function GetParam(n)
	if n == 1 then
		return q.Location, l.PlayerRC, q.Location, l.PlayerVC, q.Count, 1
	elseif n == 2 then
		return q.Location, l.PlayerRC, q.Other, o.NotAttackTarget, q.Count, 1
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
		return true
	end
	return false
end

function Activate(n)
	if n == 1 then
		obj.ChooseAddBattleOnlyPower(1, 5000)
		if obj.Exists(2) then
			obj.ChooseAddToHand(2)
		end
	end
	return 0
end